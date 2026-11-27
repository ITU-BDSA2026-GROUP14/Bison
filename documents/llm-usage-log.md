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

09-10-2026 Hannibal Marcellus Munk (Claude Code):
- Purpose: Debugged a "cannot convert IEnumerable<ObservationDTO> to ObservationDTO" compile error in PostRepository.GetObservation.
- Reason: To understand why the method returned a sequence instead of a single DTO and how to fix it, and to choose how to handle a not-found observation (null vs. exception).

21-10-2026 Hannibal Marcellus Munk (Claude Code):
- Purpose: Reviewed the DTO/PostRepository refactor across the repo (build errors, pages, DbInitializer, tests) and listed what was still missing; advised on showing author names in Public.cshtml and diagnosed post-merge build errors caused by renamed model properties and explained the remaining compiler warnings.
- Reason: To catch leftover references to the old data model before merging and to choose how to load authors for a list of observations.

30-10-2026 Daniel Kyhl:
Claude was used to diagnose the build errors after starting ASP.NET Core Identity on the feat-identity branch. It helped us see that BisonDBContext was missing the Identity using directive, that ApplicationUser and AppDbContext didn't exist, and that the Identity package versions didn't match .NET 8.

11-11-2026 Hannibal Marcellus Munk:
Claude was used to assess whether the project follows the onion architecture after the Week 11 refactor, and to teach us what the onion architecture is. It helped us see which dependencies already point the right way and where the interfaces, services and tests still deviate from the pattern.

13-11-2026 Hannibal Marcellus Munk:
Claude was used to explain how to fix the CS8602 warning in _LoginPartial.cshtml, and how to run and log in to the app locally with GitHub OAuth.

13-11-2026 Simon Skouboe:
Claude was used to diagnose a "SQLite Error 19: FOREIGN KEY constraint failed" when posting a new observation. It helped us see that the local bison.db was created by EnsureCreated with the old schema (Posts.AuthorId as an integer referencing the Authors table), which no longer matches the Identity-based Author model.

20-11-2026 Daniel Kyhl:
Claude was used to guide the implementation of the Pictures, Licenses and "about me" (GDPR) features: uploading pictures and storing them in their own Image table, choosing and showing a Creative Commons license for each picture, and an About me page that lists a user's account data and all their observations, comments and proposals. It also helped us fix build and merge errors after the onion refactor and work through the review feedback on the Pictures PR.
20-11-2026 Simon Skouboe:
Claude was used to diagnose a "SQLite Error 1: no such column: i.License" when opening the public timeline. It helped us see that the local bison.db had been created by EnsureCreated before the AddImageLicense migration, so its Images table had no License column, and that removing the old database lets the app recreate it with the current schema.

20-11-2026 Hannibal Marcellus Munk:
Claude was used to research bird sound sources (Xeno-canto, Macaulay Library, GBIF) for a "hear the taxon" feature and to check how well they would cover our Pelecaniformes taxa. It then set the scientific names for the 22 species and subspecies in DbInitializer.cs.

20-11-2026 Hannibal Marcellus Munk:
Claude was used to diagnose why GitHub Desktop could not fetch the Bison repo ("mmap failed: Operation canceled"). It helped us identify the likely causes: the repo lives in an iCloud-synced Desktop folder, the disk is nearly full, and there are leftover tmp_pack files in .git.

25-11-2026 Hannibal:
Copilot was used to diagnose why the Xeno-canto API returned an HTML "Something went wrong!" page instead of JSON. It identified the response as an Anubis anti-bot proof-of-work challenge served by the reverse proxy in front of xeno-canto.org, which .NET's HttpClient cannot solve, and recommended contacting Xeno-canto or using an approved API access method.
20-11-2026 Simon Skouboe:
Claude was used to explain how to design the "Forget me" feature and whether its logic belongs in PostService or PostRepository. It helped us understand the split: the service decides what forgetting means, the repository changes the data (soft delete and detaching observations), and the page model signs the user out.

27-11-2026 Filip Jacobsen:
Claude was used to review the repository changes since the Dafny taxon filter (assignment 1.a/1.b) was first written and to guide us step by step through implementing the filter feature. It helped us see that the verified filter and generated C# already existed, and helped us add the filter service, the taxon selector on the public timeline and Dafny verification and compilation in the GitHub workflow.

27-11-2026 Filip Jacobsen:
Claude was used to diagnose a "cannot lock ref ORIG_HEAD ... Operation timed out" error when running git pull. It helped us see that the file was an iCloud placeholder (dataless) because the repo lives in the iCloud-synced Desktop folder, and that removing it lets git recreate it.
