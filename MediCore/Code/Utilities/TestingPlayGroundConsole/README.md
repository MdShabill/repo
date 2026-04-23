## Project/File: MediCoreEMR.TestingPlayGroundConsole
### Description: 
   * Why:  Since, Implementing  the actual Unit Testing frameworks (eg: NUnit, XUnit) is not feasible in 
            MediCoreEMR(Classic) for timebeing. 
         We needed some library to achieve the UnitTesting of calculation & other functions with realistic test data.
   * What:
      *  This library contains Testing classes where we can Add the Actual functions from Different Modules.
       Also we can create Mock-Data and test different scenarios.
       After successfully testing in here, we can simply Copy-Paste that function into Actual impelementation (MediCoreEMR libraries).

      * We're referring the MediCoreEMR.ServerModels so that we can create mock data using our Actual Models.
      *  This is a Console APP, so it can run independent and is very very lightweight than the MediCoreEMR (WebProject) itself.

   * How:
     * TestClasses: TestClasses contains the actual Testing Logic to test any Logic or Functions of MediCoreEMR recreating 
                 those functions as TestFunctions.
     * TestFunctions: TestFunctions will contain the actual Logic from the MediCoreEMR libraries.s
     * MockDataProviders: MockDataProviders contains actual test data referring MediCoreEMR.ServerModel which provides test data to * TestFunctions
     * TestRunner:  Main Function (in Program.cs) can be used to run the Tests in All TestClasses
     
     * Example of Folder Structure:

        ```
        ├── Modules
        │   ├── ADT
        │   │   ├── TestClasses
        │   │   ├── Models
        │   │   ├── MockDataProviders

        ```

<span style="color:red;font-weight:bold;"> Note: </span> 
<span style="color:blue"> This framework doesn't give Pass/Fail status. Rather we have to check manually by running the required functions.</span>     