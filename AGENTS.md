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
2. Use this format:

   ```
   DD-MM-YYYY Developer Name(s) (LLM tool):
   - Purpose: What the LLM was used for (the task, feature or files involved).
   - Reason: Why an LLM was used (e.g. to learn a concept, debug an error, generate boilerplate).
   ```

   Example:

   ```
   07-10-2026 Simon Skouboe (Claude Code):
   - Purpose: Wrote CLAUDE.md and AGENTS.md instructing LLM agents to log their usage in this file.
   - Reason: To make LLM usage logging automatic, as required by issue #24.
   ```

3. Use today's date as `DD-MM-YYYY`. For the name, use the developer you are working with.
   Take it from `git config user.name` only if that is a real person's name; otherwise ask.
   Never write your own name or an email address.
4. Keep each field to 1–3 sentences. Never include secrets, credentials or personal data.
5. If you commit, put the log change in the same commit/PR as the work.
   Otherwise, leave it uncommitted and tell the developer you updated the log.