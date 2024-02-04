using Azure.Messaging.ServiceBus;

string connectionString = "Endpoint=sb://aznamespacejlavudi.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=+RSYIyLL0ndG063vfn09XhaxPSxRIITyu+ASbGFde58=;EntityPath=simpletopic";
string topic = "simpletopic";
string subscription = "SimpleSubscription";

ServiceBusClient serviceBusClient;
ServiceBusProcessor serviceBusProcessor;

serviceBusClient = new ServiceBusClient(connectionString);

serviceBusProcessor = serviceBusClient.CreateProcessor(topic, subscription,new ServiceBusProcessorOptions());

//Create Handlers

//Process message handlers
async Task MessageHandler(ProcessMessageEventArgs args)
{
    string messageBody = args.Message.Body.ToString();
    Console.WriteLine($"Received: {messageBody} from subscription {subscription}");

    //Receive the message and clear the messages from subscription

    await args.CompleteMessageAsync(args.Message);


}

// Error Handlers
 Task ErrorHandlers(ProcessErrorEventArgs args)
{
    Console.WriteLine($"Error occured while receving the messages : {args.Exception.Message}");

    return Task.CompletedTask;
}

//serviceBusClient.CreateReceiver(topic, connectionString);

try
{
    //Add handler to process messages
    serviceBusProcessor.ProcessMessageAsync += MessageHandler;

    //Add Handler to process Errors
    serviceBusProcessor.ProcessErrorAsync += ErrorHandlers;

    //start processing messages
    serviceBusProcessor.StartProcessingAsync();

    Console.WriteLine("Wait for minute and press a key");
    Console.ReadLine();

    Console.WriteLine("Stopping receiving messages...");
    //Stop Processing messages
    serviceBusProcessor.StopProcessingAsync();
    Console.WriteLine("Stop receiving messages...");
}
finally
{
    //Calling the dispose 
    await serviceBusProcessor.DisposeAsync();
    await  serviceBusClient.DisposeAsync();
}
