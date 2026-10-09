21-09-2026 Daniel Kyhl:
Claude was used as an assistant for understanding and building the TaxonTree, used mostly for understanding the structure of the tree as well as the syntax of C#.

30-09-2026 Hannibal Marcellus Munk, Simon Skouboe, Filip Sejr:
Claude was used to teach us the best practices of making unit tests for Razor Pages, and taught us to use the Microsoft.AspNetCore.Mvc.Testing package for arranging the setup for our unit tests.

07-10-2026 Filip Sejer:
Claude was used to plan and guide assignment 1.b for Week 6. It helped us understand what DbInitializer.cs requires from the data model, and helped structure the new files DTOs, IPostRepository, and PostRepository without the BisonDBContext structure.

07-10-2026 Hannibal Marcellus Munk:
Implemented EFCore into project as BisonContext.cs, which is refactored from DbFacade.cs.
After refactor, I used Claude AI to learn how Program.cs would need to be modified to support BisonContext.cs instead of the former DbFacade.cs

09-10-2026 Simon Skouboe:
Claude was used to guide assignment 1.a for Week 7. It explained how to specify and verify the Dafny FilterBy function with {:extern} Taxon and Observation classes, wrap it in a second module and generate C# code that can be called from Bison.Razor, and it reviewed our unit test for the filter.