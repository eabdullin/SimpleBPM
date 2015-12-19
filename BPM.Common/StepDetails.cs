namespace BPM.Common
{
    public class StepDetails
    {
        public string Key { get; set; }
        /// <summary>
        /// Название шага
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Доступные роли
        /// </summary>
        public string[] Roles { get; set; }

        /// <summary>
        /// Количество процессов находящихся на этом шаге
        /// </summary>
        public int ProcessInstanceCount { get; set; }

        /// <summary>
        /// Отображение карточки на этом шаге
        /// </summary>
        public string View { get; set; }

        public string StepLabel { get; set; } 
    }
}
