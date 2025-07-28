using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Producer;
using System.Collections;
using System.Text;
using System.Text.Json;

internal class Program
{
    private const string ConnectionString = "";
    private const string EventHubName = "";

    public static void Main(string[] args)
    {
        Program m = new Program();

        Console.WriteLine("Select operation:");
        Console.WriteLine("1. Read events");
        Console.WriteLine("2. Write test events");
        var choice = Console.ReadLine();

        if (choice == "1")
        {
            m.read().GetAwaiter().GetResult();
        }
        else if (choice == "2")
        {
            Console.WriteLine("Enter number of test messages to send:");
            if (int.TryParse(Console.ReadLine(), out int count))
            {
                m.WriteWithTypedHeaders(count).GetAwaiter().GetResult();
            }
            else
            {
                Console.WriteLine("Invalid input");
            }
        }
    }

    public async Task read()
    {
        var consumer = new EventHubConsumerClient(
            EventHubConsumerClient.DefaultConsumerGroupName,
            ConnectionString,
            eventHubName: EventHubName);

        Console.WriteLine("Starting to read events...");

        try
        {
            await foreach (PartitionEvent receivedEvent in consumer.ReadEventsAsync())
            {
                Console.WriteLine($"Event received from partition {receivedEvent.Partition.PartitionId}:");
                Console.WriteLine($"\tSequence number: {receivedEvent.Data.SequenceNumber}");
                Console.WriteLine($"\tTime: {receivedEvent.Data.EnqueuedTime.ToUnixTimeMilliseconds()}");
                Console.WriteLine($"\tBody: {Encoding.UTF8.GetString(receivedEvent.Data.EventBody)}");
                Console.WriteLine($"\tKey: {receivedEvent.Data.PartitionKey}");
                Console.WriteLine($"\tOffset: {receivedEvent.Data.OffsetString}");

                //Console.WriteLine($"\tProperties: {string.Join(", ", receivedEvent.Data.Properties.Select(kv => $"{kv.Key}:{Encoding.UTF8.GetString((byte[]) kv.Value)}"))}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading events: {ex.Message}");
        }
        finally
        {
            await consumer.CloseAsync();
        }
    }

    public async Task WriteWithTypedHeaders(int eventCount)
    {
        await using (var producer = new EventHubProducerClient(ConnectionString, EventHubName))
        {
            Console.WriteLine($"Preparing to send {eventCount} messages with typed headers...");

            try
            {
                using EventDataBatch eventBatch = await producer.CreateBatchAsync();

                for (int i = 1; i <= eventCount; i++)
                {
                    using var stream = new MemoryStream(Encoding.UTF8.GetBytes("Stream data"));
                        // Create test data
                    var testData = new
                    {
                        MessageId = i,
                        Timestamp = DateTime.UtcNow.ToString("o"),
                        DeviceId = $"DEV-{Guid.NewGuid().ToString().Substring(0, 8)}",
                        Temperature = new Random().NextDouble() * 100,
                        Status = i % 2 == 0 ? "Active" : "Standby"
                    };

                    string jsonData = JsonSerializer.Serialize(testData);
                    var eventData = new EventData(Encoding.UTF8.GetBytes(jsonData));

                    // 添加类型化Header（Properties）
                    AddTypedHeaders(eventData, stream);

                    if (!eventBatch.TryAdd(eventData))
                    {
                        await producer.SendAsync(eventBatch);
                        Console.WriteLine($"Sent a batch of {eventBatch.Count} messages");

                        eventBatch.Dispose();
                        using var newBatch = await producer.CreateBatchAsync();

                        if (!newBatch.TryAdd(eventData))
                        {
                            throw new Exception($"Event #{i} is too large for the batch");
                        }
                    }
                }

                if (eventBatch.Count > 0)
                {
                    await producer.SendAsync(eventBatch);
                    Console.WriteLine($"Sent final batch of {eventBatch.Count} messages");
                }

                Console.WriteLine($"Successfully sent {eventCount} messages with typed headers");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    private void AddTypedHeaders(EventData eventData, Stream stream)
    {
        // 基础类型
        eventData.Properties.Add("header_string", "Sample string");
        eventData.Properties.Add("header_bool", true);
        eventData.Properties.Add("header_byte", (byte)42);
        eventData.Properties.Add("header_sbyte", (sbyte)-42);

        // 整数类型
        eventData.Properties.Add("header_short", (short)-1000);
        eventData.Properties.Add("header_ushort", (ushort)1000);
        eventData.Properties.Add("header_int", -100000);
        eventData.Properties.Add("header_uint", 100000u);
        eventData.Properties.Add("header_long", -10000000000L);
        eventData.Properties.Add("header_ulong", 10000000000UL);

        // 浮点/高精度
        eventData.Properties.Add("header_float", 3.14f);
        eventData.Properties.Add("header_double", 3.1415926535);
        eventData.Properties.Add("header_decimal", Decimal.Parse("1234567890.1234567890"));

        // 特殊类型
        eventData.Properties.Add("header_char", 'X');
        eventData.Properties.Add("header_guid", Guid.NewGuid());
        eventData.Properties.Add("header_datetime", DateTime.UtcNow);
        eventData.Properties.Add("header_datetimeoffset", DateTimeOffset.Now);
        eventData.Properties.Add("header_timespan", TimeSpan.FromHours(2.5));
        eventData.Properties.Add("header_uri", new Uri("https://example.com"));

        // 二进制数据
        eventData.Properties.Add("header_bytearray", new byte[] { 0x01, 0x02, 0x03 });
        eventData.Properties.Add("header_stream", stream);
    }
}
