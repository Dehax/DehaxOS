using System;
using System.Text;

namespace DehaxOS.Scheduler
{
    /// <summary>
    /// Возможные состояния процесса.
    /// </summary>
    public enum ProcessState
    {
        /// <summary>
        /// Процесс создан.
        /// </summary>
        Created,
        /// <summary>
        /// Процесс готов к исполнению.
        /// </summary>
        Ready,
        /// <summary>
        /// Процесс выполняется.
        /// </summary>
        Running,
        /// <summary>
        /// Процесс ожидает получения доступа к ресурсу.
        /// </summary>
        Waiting,
        /// <summary>
        /// Процесс завершён.
        /// </summary>
        Killed
    }

    /// <summary>
    /// Приоритет процесса.
    /// </summary>
    public enum ProcessPriority
    {
        /// <summary>
        /// Системный.
        /// </summary>
        System = 0,
        /// <summary>
        /// Высокий.
        /// </summary>
        High = 1,
        /// <summary>
        /// Обычный.
        /// </summary>
        Normal = 2,
        /// <summary>
        /// Низкий.
        /// </summary>
        Low = 3
    }

    /// <summary>
    /// Представляет процесс.
    /// </summary>
    public class Process
    {
        /// <summary>
        /// Состояние процесса.
        /// </summary>
        public ProcessState State { get; set; }
        /// <summary>
        /// ID процесса.
        /// </summary>
        public int PID { get; private set; }
        /// <summary>
        /// ID родительского процесса. Значение равно -1, если процесс не имеет родителя.
        /// </summary>
        public int PPID
        {
            get
            {
                if (Parent == null)
                {
                    return -1;
                }

                return Parent.PID;
            }
        }
        /// <summary>
        /// Родительский процесс. Значение содержит null, если процесс не имеет родителя.
        /// </summary>
        public Process Parent { get; private set; }
        /// <summary>
        /// Время работы CPU, необходимое процессу.
        /// </summary>
        public int CPUburstTime { get; private set; }
        /// <summary>
        /// Время работы I/O, необходимое процессу.
        /// </summary>
        public int IOburstTime { get; private set; }

        public ProcessPriority Priority { get; set; }

        private Random _random;

        /// <summary>
        /// Создаёт процесс с заданным ID и временными характеристиками.
        /// </summary>
        /// <param name="pid">ID процесса.</param>
        /// <param name="cpuBurstTime">Время работы CPU, необходимое процессу.</param>
        /// <param name="ioBurstTime">Время работы I/O, необходимое процессу.</param>
        public Process(int pid, int cpuBurstTime, int ioBurstTime)
        {
            _random = new Random((int)(DateTime.UtcNow.Ticks % int.MaxValue));

            CPUburstTime = cpuBurstTime;
            IOburstTime = ioBurstTime;

            Priority = ProcessPriority.Normal;

            PID = pid;
            State = ProcessState.Created;
        }

        /// <summary>
        /// Создаёт процесс с заданным ID, временными характеристиками и приоритетом.
        /// </summary>
        /// <param name="pid">ID процесса.</param>
        /// <param name="cpuBurstTime">Время работы CPU, необходимое процессу.</param>
        /// <param name="ioBurstTime">Время работы I/O, необходимое процессу.</param>
        /// <param name="priority">Приоритет процесса.</param>
        public Process(int pid, int cpuBurstTime, int ioBurstTime, ProcessPriority priority)
            : this(pid, cpuBurstTime, ioBurstTime)
        {
            Priority = priority;
        }

        /// <summary>
        /// Создаёт процесс с заданным ID процесса, временными характеристиками и родительским процессом.
        /// </summary>
        /// <param name="pid">ID процесса.</param>
        /// <param name="cpuBurstTime">Время работы CPU, необходимое процессу.</param>
        /// <param name="ioBurstTime">Время работы I/O, необходимое процессу.</param>
        /// <param name="priority">Приоритет процесса.</param>
        /// <param name="parent">Родительский процесс.</param>
        public Process(int pid, int cpuBurstTime, int ioBurstTime, ProcessPriority priority, Process parent)
            : this(pid, cpuBurstTime, ioBurstTime, priority)
        {
            Parent = parent;
        }

        /// <summary>
        /// Выполнить процесс на CPU в течение заданного количества времени (обычно значение равно интервалу тика системного таймера).
        /// </summary>
        /// <param name="cpuTime"></param>
        public void RunOnCPU(int cpuTime)
        {
            CPUburstTime -= cpuTime;

            if (IOburstTime > 0)
            {
                if (_random.Next(1, 100) > 80)
                {
                    State = ProcessState.Waiting;
                }
            }

            if (CPUburstTime <= 0)
            {
                State = ProcessState.Killed;
            }
        }

        /// <summary>
        /// Выполнить обработку I/O процесса в течение заданного количества времени.
        /// </summary>
        /// <param name="ioTime"></param>
        public void RunOnIO(int ioTime)
        {
            IOburstTime -= ioTime;

            if (IOburstTime <= 0)
            {
                State = ProcessState.Ready;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("PID = ");
            sb.Append(PID);
            sb.Append(", CPU = ");
            sb.Append(CPUburstTime);
            sb.Append(", I/O = ");
            sb.Append(IOburstTime);
            sb.Append(", State = ");
            sb.Append(State);
            sb.Append(", Priority = ");
            sb.Append(Priority);

            return sb.ToString();
        }
    }
}
