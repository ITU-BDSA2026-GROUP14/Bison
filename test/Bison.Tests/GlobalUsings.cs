global using Xunit;

/**
    * This fixes the bug where xUnit tests are run in parallel,
    * which can cause issues with shared resources like files or databases.
    * By disabling test parallelization, we ensure that tests run sequentially, 
    * preventing potential conflicts and ensuring consistent test results.
*/
[assembly: CollectionBehavior(DisableTestParallelization = true)]