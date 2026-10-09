# AGENTS.md

Instructions for AI coding agents (Claude Code, GitHub Copilot, Codex, Cursor, etc.) working in this repository.

## Log your LLM usage (required)

Every time you are used on this project, record it in `documents/llm-usage-log.md`.
This applies to all kinds of help: writing code, answering questions, debugging, reviewing and explaining.

### When
- Add one entry per session/task, before your final response.
- If an entry for the same developer, date and task already exists, update it instead of adding a duplicate.

### How
1. Append the entry at the bottom of `documents/llm-usage-log.md`, with a blank line before it.
   Never edit or delete other entries.
2. Use this format: the date and the developer name(s) on the first line, followed by one short paragraph on the next line that starts with "<LLM name> was used to ..." and says what it was used for and why.

   ```
   DD-MM-YYYY Developer Name(s):
   <LLM name> was used to <what it helped with>. <Optionally: what it helped us understand or why it was needed>.
   ```

   Example:

   ```
   07-10-2026 Filip Sejer:
   Claude was used to plan and guide assignment 1.b for Week 6. It helped us understand what DbInitializer.cs requires from the data model, and helped structure the new files DTOs, IPostRepository, and PostRepository.
   ```

3. Use today's date as `DD-MM-YYYY`. For the name, use the developer you are working with.
   Take it from `git config user.name` only if that is a real person's name; otherwise ask.
   Never write your own name or an email address.
4. Keep the paragraph to 1–3 sentences. Never include secrets, credentials or personal data.
5. If you commit, put the log change in the same commit/PR as the work.
   Otherwise, leave it uncommitted and tell the developer you updated the log.