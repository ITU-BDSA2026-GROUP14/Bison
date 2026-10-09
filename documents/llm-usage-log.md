21-09-2026 Daniel Kyhl:
Claude was used as an assistant for understanding and building the TaxonTree, used mostly for understanding the structure of the tree as well as the syntax of C#.

30-09-2026 Hannibal Marcellus Munk, Simon Skouboe, Filip Sejr:
Claude was used to teach us the best practices of making unit tests for Razor Pages, and taught us to use the Microsoft.AspNetCore.Mvc.Testing package for arranging the setup for our unit tests.

07-10-2026 Filip Sejer:
Claude was used to plan and guide assignment 1.b for Week 6. It helped us understand what DbInitializer.cs requires from the data model, and helped structure the new files DTOs, IPostRepository, and PostRepository without the BisonDBContext structure.

07-10-2026 Hannibal Marcellus Munk:
Implemented EFCore into project as BisonContext.cs, which is refactored from DbFacade.cs.
After refactor, I used Claude AI to learn how Program.cs would need to be modified to support BisonContext.cs instead of the former DbFacade.cs

09-10-2026 Hannibal Marcellus Munk (Claude Code):
- Purpose: Debugged a "cannot convert IEnumerable<ObservationDTO> to ObservationDTO" compile error in PostRepository.GetObservation.
- Reason: To understand why the method returned a sequence instead of a single DTO and how to fix it, and to choose how to handle a not-found observation (null vs. exception).
