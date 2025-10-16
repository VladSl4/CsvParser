Number of rows inserted: 29889
To handle a 10GB CSV file, the current approach of reading all data into a List<TripRecord> would certainly cause an OutOfMemoryException. 
The architecture must be changed to process the data in a streaming fashion with constant, low memory usage.
The most direct evolution of the current gRPC-based system would be to use client-side streaming combined with batched database insertions.
Instead of sending one large message with all records, the ParserService would initiate a client-streaming gRPC call. 
It would then read the CSV file line-by-line and send each record (or a small chunk of, say, 1,000 records) as a separate message in the stream.
On the server side, the DataProvider would receive records from this stream. 
It would accumulate them in a small, in-memory list. 
Once the list reaches a predefined batch size (e.g., 50,000 records), the service performs a BulkInsertAsync on that batch. 
After the insertion, the list is cleared, and the process of accumulating the next batch begins.
This combination ensures that neither the parsing client nor the database service ever holds the entire 10GB file in memory. 
The memory footprint remains small and constant throughout the entire operation, regardless of the input file's size.

One way or another, all the methods reduce to inserting the rows in chunks, no matter the delivery types, whether it`s AMQP, Streaming etc.
