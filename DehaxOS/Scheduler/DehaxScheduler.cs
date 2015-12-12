using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DehaxOS.Scheduler
{
    /// <summary>
    /// Представляет планировщик ОС DehaxOS.
    /// </summary>
    public class DehaxScheduler
    {
        #region Константы
        private const int QUEUES_NUMBER = 4;
        /// <summary>
        /// Интервал системного таймера в мс, равен одному кванту времени планировщика.
        /// </summary>
        private const int SYSTEM_TIMER_INTERVAL = 20;
        private const int DEBUG_DELAY = 10;
        #endregion

        public bool IsRunning { get; set; }

        /// <summary>
        /// Характеристики квантов времени для очереди процессов.
        /// </summary>
        struct QueueTime
        {
            public int quantumTime;
            public int quantumNumber;
        }

        /// <summary>
        /// Характеристики квантов времени для каждой очереди процессов.
        /// </summary>
        private readonly QueueTime[] _queuesTime = new QueueTime[QUEUES_NUMBER]
        {
            new QueueTime()
            {
                quantumTime = 4,
                quantumNumber = 4
            },
            new QueueTime()
            {
                quantumTime = 3,
                quantumNumber = 3
            },
            new QueueTime()
            {
                quantumTime = 3,
                quantumNumber = 2
            },
            new QueueTime()
            {
                quantumTime = 2,
                quantumNumber = 1
            }
        };
        /// <summary>
        /// Очереди процессов планировщика.
        /// </summary>
        private LinkedList<Process>[] _processesQueues = new LinkedList<Process>[4]
        {
            new LinkedList<Process>(),
            new LinkedList<Process>(),
            new LinkedList<Process>(),
            new LinkedList<Process>()
        };
        /// <summary>
        /// Процессы, ожидающие создания.
        /// </summary>
        private Queue<Process> _processesPendingCreation = new Queue<Process>();
        private Queue<Process> _processesWaiting = new Queue<Process>();


        public DehaxScheduler()
        {

        }

        /// <summary>
        /// Запускает работу планировщика и обработку процессов.
        /// </summary>
        public void Start(BackgroundWorker worker)
        {
            IsRunning = true;

            int currentQueueIndex = 4;
            Process selectedProcess = null;
            int numQuantumElapsed = 0;
            int numTimerTicksElapsed = 0;
            StringBuilder sb = new StringBuilder();

            while (IsRunning)
            {
                sb.Append(" W || ");
                foreach (Process process in _processesWaiting)
                {
                    sb.Append(process.PID);
                    sb.Append(" | ");
                }
                sb.AppendLine();

                for (int i = 0; i < _processesQueues.Length; i++)
                {
                    LinkedList<Process> queue = _processesQueues[i];

                    if (i == currentQueueIndex)
                    {
                        sb.Append('>');
                    }
                    else
                    {
                        sb.Append(' ');
                    }

                    sb.Append(i);
                    sb.Append(" || ");

                    foreach (var process in queue)
                    {
                        sb.Append(process.PID);
                        sb.Append(" | ");
                    }

                    sb.AppendLine();
                }

                if (_processesPendingCreation.Count > 0)
                {
                    // Есть процессы, ожидающие создания.
                    // Переместить из очереди СОЗДАНЫ -> ГОТОВЫЕ.

                    Process creatingProcess = _processesPendingCreation.Dequeue();
                    int queueIndex = (int)creatingProcess.Priority;
                    creatingProcess.State = ProcessState.Ready;
                    
                    if (creatingProcess.State != ProcessState.Killed)
                    {
                        if (!_processesQueues[queueIndex].Contains(creatingProcess))
                        {
                            _processesQueues[queueIndex].AddLast(creatingProcess);
                        }

                        if (queueIndex < currentQueueIndex)
                        {
                            // Процесс вытестил выполняющийся.
                            selectedProcess = creatingProcess;
                            currentQueueIndex = queueIndex;
                        }
                    }
                }

                if (selectedProcess == null)
                {
                    // Процесс не выбран, выбираем следующий процесс.
                    Process process = null;

                    if (_processesQueues[currentQueueIndex].Count > 0)
                    {
                        process = _processesQueues[currentQueueIndex].First.Value;

                        if (process.State == ProcessState.Killed)
                        {
                            _processesQueues[currentQueueIndex].RemoveFirst();
                            process = null;
                        }
                    }

                    selectedProcess = process;
                }

                Queue<Process> needRestoreWaiting = new Queue<Process>();

                int maxWaiting = _processesWaiting.Count;
                for (int i = 0; i < maxWaiting; i++)
                {
                    Process process = _processesWaiting.Dequeue();

                    if (process.State == ProcessState.Killed)
                    {
                        continue;
                    }

                    process.RunOnIO(SYSTEM_TIMER_INTERVAL);

                    if (process.State == ProcessState.Waiting)
                    {
                        if (!needRestoreWaiting.Contains(process))
                        {
                            needRestoreWaiting.Enqueue(process);
                        }
                    }
                    else if (process.State == ProcessState.Ready)
                    {
                        if (!_processesQueues[(int)process.Priority].Contains(process))
                        {
                            _processesQueues[(int)process.Priority].AddLast(process);
                        }
                    }
                }

                foreach (Process process in needRestoreWaiting)
                {
                    if (!_processesWaiting.Contains(process))
                    {
                        _processesWaiting.Enqueue(process);
                    }
                }

                if (selectedProcess != null)
                {
                    selectedProcess.State = ProcessState.Running;
                    selectedProcess.RunOnCPU(SYSTEM_TIMER_INTERVAL);

                    numTimerTicksElapsed++;

                    if (selectedProcess.State == ProcessState.Waiting)
                    {
                        _processesQueues[currentQueueIndex].Remove(selectedProcess);
                        if (!_processesWaiting.Contains(selectedProcess))
                        {
                            _processesWaiting.Enqueue(selectedProcess);
                        }

                        selectedProcess = null;
                        numTimerTicksElapsed = 0;
                        numQuantumElapsed++;
                    }
                    else if (selectedProcess.State == ProcessState.Killed)
                    {
                        _processesQueues[currentQueueIndex].Remove(selectedProcess);
                        selectedProcess = null;
                        numTimerTicksElapsed = 0;
                        numQuantumElapsed++;
                    }
                    else
                    {
                        selectedProcess.State = ProcessState.Ready;
                    }

                    if (numTimerTicksElapsed >= _queuesTime[currentQueueIndex].quantumTime)
                    {
                        numTimerTicksElapsed = 0;

                        // Прокрутить очередь RR, переключиться на следующий процесс.
                        if (_processesQueues[currentQueueIndex].Count > 0)
                        {
                            Process process = _processesQueues[currentQueueIndex].First.Value;
                            _processesQueues[currentQueueIndex].RemoveFirst();
                            if (!_processesQueues[(int)process.Priority].Contains(process))
                            {
                                _processesQueues[(int)process.Priority].AddLast(process);
                            }

                            if (_processesQueues[currentQueueIndex].Count > 0)
                            {
                                selectedProcess = _processesQueues[currentQueueIndex].First.Value;

                                if (selectedProcess.State == ProcessState.Killed)
                                {
                                    _processesQueues[currentQueueIndex].RemoveFirst();
                                    selectedProcess = null;
                                }
                            }

                            while (selectedProcess == null)
                            {
                                if (_processesQueues[currentQueueIndex].Count > 0)
                                {
                                    selectedProcess = _processesQueues[currentQueueIndex].First.Value;

                                    if (selectedProcess.State == ProcessState.Killed)
                                    {
                                        _processesQueues[currentQueueIndex].RemoveFirst();
                                        selectedProcess = null;
                                    }
                                }
                            }
                        }
                        else
                        {
                            selectedProcess = null;
                        }

                        numQuantumElapsed++;
                    }

                    if (numQuantumElapsed >= _queuesTime[currentQueueIndex].quantumNumber)
                    {
                        numQuantumElapsed = 0;

                        currentQueueIndex++;

                        if (currentQueueIndex >= QUEUES_NUMBER)
                        {
                            currentQueueIndex = 0;
                        }
                    }
                }
                else
                {
                    bool canExit = true;

                    foreach (LinkedList<Process> queue in _processesQueues)
                    {
                        foreach (Process process in queue)
                        {
                            canExit = false;
                            break;
                        }

                        if (!canExit)
                        {
                            break;
                        }
                    }

                    foreach (Process process in _processesPendingCreation)
                    {
                        canExit = false;
                        break;
                    }

                    foreach (Process process in _processesWaiting)
                    {
                        canExit = false;
                        break;
                    }

                    if (canExit)
                    {
                        IsRunning = false;
                    }

                    numTimerTicksElapsed = 0;
                    numQuantumElapsed = 0;
                    currentQueueIndex++;

                    if (currentQueueIndex >= QUEUES_NUMBER)
                    {
                        currentQueueIndex = 0;
                    }
                }

                worker.ReportProgress(0, sb.ToString());
                sb.Clear();

                System.Threading.Thread.Sleep(SYSTEM_TIMER_INTERVAL * DEBUG_DELAY);
            }
        }

        /// <summary>
        /// Посылает планировщику запрос на создание нового процесса.
        /// </summary>
        /// <param name="pid">ID процесса.</param>
        /// <param name="cpuBurstTime">Время CPU, необходимое процессу на выполнение.</param>
        /// <param name="ioBurstTime">Время I/O, необходимое процессу на выполнение.</param>
        /// <param name="priority">Приоритет процесса.</param>
        /// <param name="parent">Родительский процесс.</param>
        /// <returns>true, если процесс успешно добавлен, и false, иначе.</returns>
        public bool AddProcess(int pid, int cpuBurstTime, int ioBurstTime, ProcessPriority priority = ProcessPriority.Normal, Process parent = null)
        {
            if (ProcessExists(pid))
            {
                return false;
            }

            Process process = new Process(pid, cpuBurstTime, ioBurstTime, priority, parent);

            _processesPendingCreation.Enqueue(process);

            return true;
        }

        /// <summary>
        /// Изменяет приоритет процесса в системе.
        /// </summary>
        /// <param name="pid">ID процесса, приоритет которого нужно изменить.</param>
        /// <param name="priority">Значение нового приоритета.</param>
        /// <returns>true, если приоритет процесса успешно изменён, и false, иначе.</returns>
        public bool ChangeProcessPriority(int pid, ProcessPriority priority)
        {
            Process process;

            if ((process = FindProcess(pid)) == null)
            {
                return false;
            }

            process.Priority = priority;

            return true;
        }

        /// <summary>
        /// Посылает запрос на уничтожение процесса.
        /// </summary>
        /// <param name="pid">ID процесса, который нужно уничтожить.</param>
        /// <returns>true, если процесс назначен на уничтожение, и false, иначе.</returns>
        public bool KillProcess(int pid)
        {
            Process process;

            if ((process = FindProcess(pid)) == null)
            {
                return false;
            }

            process.State = ProcessState.Killed;

            //foreach (LinkedList<Process> queue in _processesQueues)
            //{
            //    if (queue.Remove(process))
            //    {
            //        return true;
            //    }
            //}

            return true;
        }

        /// <summary>
        /// Проверяет, существует ли уже в системе процесс с заданным PID.
        /// </summary>
        /// <param name="pid">ID процесса, существование которого необходимо проверить.</param>
        /// <returns>true, если процесс существует, и false, иначе.</returns>
        private bool ProcessExists(int pid)
        {
            foreach (Process process in _processesPendingCreation)
            {
                if (process.PID == pid)
                {
                    return true;
                }
            }

            foreach (LinkedList<Process> queue in _processesQueues)
            {
                foreach (Process process in queue)
                {
                    if (process.PID == pid)
                    {
                        return true;
                    }
                }
            }

            foreach (Process process in _processesWaiting)
            {
                if (process.PID == pid)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Выполняет поиск процесса по PID.
        /// </summary>
        /// <param name="pid">ID искомого процесса.</param>
        /// <returns>Возвращает найденный процесс или null, если процесса с указанным PID не существует в системе.</returns>
        private Process FindProcess(int pid)
        {
            foreach (Process process in _processesPendingCreation)
            {
                if (process.PID == pid)
                {
                    return process;
                }
            }

            foreach (LinkedList<Process> queue in _processesQueues)
            {
                foreach (Process process in queue)
                {
                    if (process.PID == pid)
                    {
                        return process;
                    }
                }
            }

            foreach (Process process in _processesWaiting)
            {
                if (process.PID == pid)
                {
                    return process;
                }
            }

            return null;
        }

        public void Clear()
        {
            foreach (var queue in _processesQueues)
            {
                queue.Clear();
            }

            _processesPendingCreation.Clear();
            _processesWaiting.Clear();

            IsRunning = false;
        }
    }
}
