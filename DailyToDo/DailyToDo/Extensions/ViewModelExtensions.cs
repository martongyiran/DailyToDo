using DailyToDo.ViewModels.Interfaces;
using System;
using System.Threading.Tasks;

namespace DailyToDo.Extensions
{
    public static class ViewModelExtensions
    {
        public static Action WrapWithIsBusy(this IViewModel viewModel, Action action)
        {
            return () =>
            {
                if (viewModel.IsBusy)
                {
                    return;
                }

                viewModel.IsBusy = true;
                action();
                viewModel.IsBusy = false;
            };
        }

        public static Func<Task> WrapWithIsBusy(this IViewModel viewModel, Func<Task> task)
        {
            return async () =>
            {
                if (viewModel.IsBusy)
                {
                    throw new InvalidOperationException("Cannot load the result of a different Task<T> while busy");
                }

                viewModel.IsBusy = true;

                await task.Invoke();

                viewModel.IsBusy = false;
            };
        }

        public static Func<ValueTask> WrapWithIsBusy(this IViewModel viewModel, Func<ValueTask> task)
        {
            return async () =>
            {
                if (viewModel.IsBusy)
                {
                    throw new InvalidOperationException("Cannot load the result of a different Task<T> while busy");
                }

                viewModel.IsBusy = true;

                await task.Invoke();

                viewModel.IsBusy = false;
            };
        }

        public static Func<Task<T>> WrapWithIsBusy<T>(this IViewModel viewModel, Func<Task<T>> task)
        {
            return async () =>
            {
                if (viewModel.IsBusy)
                {
                    throw new InvalidOperationException("Cannot load the result of a different Task<T> while busy");
                }

                viewModel.IsBusy = true;

                var result = await task();

                viewModel.IsBusy = false;

                return result;
            };
        }

        public static Func<ValueTask<T>> WrapWithIsBusy<T>(this IViewModel viewModel, Func<ValueTask<T>> task)
        {
            return async () =>
            {
                if (viewModel.IsBusy)
                {
                    throw new InvalidOperationException("Cannot load the result of a different ValueTask<T> while busy");
                }

                viewModel.IsBusy = true;

                var result = await task();

                viewModel.IsBusy = false;

                return result;
            };
        }

        public static Func<TParameter, Task> WrapWithIsBusy<TParameter>(this IViewModel viewModel, Func<TParameter, Task> task)
        {
            return async parameter =>
            {
                if (viewModel.IsBusy)
                {
                    throw new InvalidOperationException("Cannot load the result of a different Task<T> while busy");
                }

                viewModel.IsBusy = true;

                await task.Invoke(parameter);

                viewModel.IsBusy = false;
            };
        }

        public static Func<TParameter, ValueTask> WrapWithIsBusy<TParameter>(this IViewModel viewModel, Func<TParameter, ValueTask> task)
        {
            return async parameter =>
            {
                if (viewModel.IsBusy)
                {
                    throw new InvalidOperationException("Cannot load the result of a different ValueTask<T> while busy");
                }

                viewModel.IsBusy = true;

                await task.Invoke(parameter);

                viewModel.IsBusy = false;
            };
        }

        public static async Task InvokeWithIsBusy(this IViewModel viewModel, Func<Task> task)
        {
            if (viewModel.IsBusy)
            {
                return;
            }

            viewModel.IsBusy = true;
            await task();
            viewModel.IsBusy = false;
        }

        public static async Task InvokeWithIsBusy(this IViewModel viewModel, Task task)
        {
            if (viewModel.IsBusy)
            {
                return;
            }

            viewModel.IsBusy = true;
            await task;
            viewModel.IsBusy = false;
        }

        public static void InvokeWithIsBusy(this IViewModel viewModel, Action action)
        {
            if (viewModel.IsBusy)
            {
                return;
            }

            viewModel.IsBusy = true;
            action();
            viewModel.IsBusy = false;
        }
    }
}
