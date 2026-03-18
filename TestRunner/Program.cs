using System;
using System.Threading.Tasks;

class TestRunnerProgram
{
    static async Task<int> Main()
    {
        try
        {
            // Test 1: GET-like logic
            var msg = WelcomeService.CreateMessage("Gaurav");
            if (msg != "Gaurav welcome back") throw new Exception($"Unexpected message: {msg}");

            // Test 2: POST-like logic (same function)
            var msg2 = WelcomeService.CreateMessage("Alice");
            if (msg2 != "Alice welcome back") throw new Exception($"Unexpected message: {msg2}");

            Console.WriteLine("All tests passed");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Test failed: " + ex.Message);
            return 1;
        }
    }
}
