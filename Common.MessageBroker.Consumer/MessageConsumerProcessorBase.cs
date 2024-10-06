using System.Collections.Immutable;
using Common.MessageBroker.Consumer.Interfaces;
using Common.Messages.PublishSubscribeMessages;
using Microsoft.Extensions.Logging;

namespace Common.MessageBroker.Consumer;

public abstract class MessageConsumerProcessorBase : IMessageConsumerProcessor
{
    private readonly ILogger<MessageConsumerProcessorBase> _logger;
    private readonly IMessageConsumer _messageConsumer;

    public MessageConsumerProcessorBase(ILogger<MessageConsumerProcessorBase> logger, IMessageConsumer messageConsumer)
    {
        _logger = logger;
        _messageConsumer = messageConsumer;
    }

    // public async Task RegisterOnMessageHandlerAndReceiveMessages()
    // {
    //     _queueClient.ReceiveAsync() RegisterMessageHandler(
    //          async (message, token) => await ReceiveMessagesAsync(message, token).ConfigureAwait(false),
    //         new MessageHandlerOptions(ExceptionReceivedHandler)
    //         {
    //             AutoComplete = false,
    //             MaxConcurrentCalls = 10
    //         }
    //     );
    //     await Task.CompletedTask;
    // }

    public async Task RunAsync()
    {
        try
        {
            while (true)
            {
                var (queueMessages, originalMessages) = await _messageConsumer.ReceiveMessagesAsync().ConfigureAwait(false);

                var immutableList = queueMessages.ToImmutableList();

                if(immutableList == null || immutableList.Count == 0)
                {
                    //
                    return;
                }

                var handledMessages = await HandleMessagesAsync(immutableList).ConfigureAwait(false);

                if (handledMessages.Any())
                {
                    var messageToDelete = originalMessages.Where(m => handledMessages.Contains(m.MessageId));
                    await _messageConsumer.DeleteMessagesAsync(messageToDelete).ConfigureAwait(false);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message");
        }
    }

    protected abstract Task<IEnumerable<string>> HandleMessagesAsync(ImmutableList<QueueMessage> qMessages);   

    // private async Task ReceiveMessagesAsync(Message message, CancellationToken token)
    // {
    //     _logger.LogInformation("Received message: {Message}", message);
        
    //     try
    //     {
    //         var msg = new QueueMessage
    //         {
    //             MessageId = message.MessageId,
    //             MessageBody = System.Text.Encoding.UTF8.GetString(message.Body)
    //         };

    //         var isCompletedSuccessfully = await HandleMessagesAsync(msg).ConfigureAwait(false);

    //         if (isCompletedSuccessfully)
    //         {
    //             await _queueClient.CompleteAsync(message.SystemProperties.LockToken).ConfigureAwait(false);
    //         }

    //         await Task.CompletedTask;
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "Error processing message");
    //     }
    // }

    // private async Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
    // {
    //     _logger.LogError(exceptionReceivedEventArgs.Exception, "Message handler encountered an exception");
    //     await Task.CompletedTask;
    // }
}


