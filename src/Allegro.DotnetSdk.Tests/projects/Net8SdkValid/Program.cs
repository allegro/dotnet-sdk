Print("a");

// if Sdk is not imported, this will raise:
// warning CS8632: The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
// error CS0103: The name 'Console' does not exist in the current context
static void Print(string? text) => Console.WriteLine("Text: " + text);