using System;
using Bocami.Practices.Decorator;
using System.Transactions;

namespace Bocami.Practices.Command.Transactions
{
    /// <summary>
    /// Decorates an ICommandHandler of TCommand inside a TransactionScope
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    public class TransactionScopeCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>, IDecorator<ICommandHandler<TCommand>>
        where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> commandHandler;

        public TransactionScopeCommandHandlerDecorator(ICommandHandler<TCommand> commandHandler)
        {
            if (commandHandler == null)
                throw new ArgumentNullException("commandHandler");

            this.commandHandler = commandHandler;
        }

        public void Handle(TCommand command)
        {
            using (var transactionScope = new TransactionScope())
            {
                commandHandler.Handle(command);

                transactionScope.Complete();
            }
        }
    }
}
