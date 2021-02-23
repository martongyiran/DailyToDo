using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DailyToDo.Mvvm
{
    public class DelayCommand : DelegateCommand
    {
        protected readonly CommandExecutionValidator ExecutionValidator = new CommandExecutionValidator();

        protected Func<bool> _canExecuteMethod;

        public DelayCommand(Action executeMethod)
            : base(executeMethod)
        {
        }

        public DelayCommand(Action executeMethod, Func<bool> canExecuteMethod)
            : base(executeMethod)
        {
            _canExecuteMethod = canExecuteMethod;
        }

        public new DelayCommand ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
        {
            _canExecuteMethod = canExecuteExpression.Compile();
            ObservesPropertyInternal(canExecuteExpression);
            return this;
        }

        public new bool CanExecute()
        {
            return _canExecuteMethod?.Invoke() ?? true;
        }

        protected override void Execute(object parameter)
        {
            if (ExecutionValidator.ExecutionIsAllowed() && CanExecute(parameter))
            {
                base.Execute(parameter);
            }
        }

        protected override bool CanExecute(object parameter)
        {
            return _canExecuteMethod?.Invoke() ?? true;
        }
    }

    public class DelayCommand<TParameter> : DelegateCommand<TParameter>
    {
        protected readonly CommandExecutionValidator ExecutionValidator = new CommandExecutionValidator();

        protected Func<TParameter, bool> _canExecuteMethod;

        public DelayCommand(Action<TParameter> executeMethod)
            : base(executeMethod)
        {
        }

        public DelayCommand(Action<TParameter> executeMethod, Func<TParameter, bool> canExecuteMethod)
            : base(executeMethod)
        {
            _canExecuteMethod = canExecuteMethod;
        }

        public new DelayCommand<TParameter> ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
        {
            var expression = Expression.Lambda<Func<TParameter, bool>>(
                canExecuteExpression.Body,
                Expression.Parameter(typeof(TParameter), "o"));
            _canExecuteMethod = expression.Compile();
            ObservesPropertyInternal(canExecuteExpression);
            return this;
        }

        protected override void Execute(object parameter)
        {
            if (ExecutionValidator.ExecutionIsAllowed() && CanExecute(parameter))
            {
                base.Execute(parameter);
            }
        }

        protected override bool CanExecute(object parameter)
        {
            return parameter switch
            {
                null => _canExecuteMethod?.Invoke(default) ?? true,
                TParameter validParameter => _canExecuteMethod?.Invoke(validParameter) ?? true,
                _ => false,
            };
        }
    }
}
