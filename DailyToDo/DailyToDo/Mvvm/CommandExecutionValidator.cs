using System;

namespace DailyToDo.Mvvm
{
    public class CommandExecutionValidator
    {
        private readonly TimeSpan _minimumExecutionInterval = TimeSpan.FromMilliseconds(500);

        private DateTime _lastExecution;

        public bool ExecutionIsAllowed()
        {
            var delta = DateTime.Now - _lastExecution;
            var allowed = delta > _minimumExecutionInterval;
            _lastExecution = DateTime.Now;

            return allowed;
        }
    }
}
