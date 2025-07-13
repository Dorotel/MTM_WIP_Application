# Ultra-Detailed Commit Message Generation Prompt

Use this prompt with your AI tool to generate the most exhaustive, precise, and maintainable commit messages possible. This prompt is designed to capture every relevant detail for future reviewers and maintainers.

---

## Prompt

> Generate an **ultra-detailed, professional commit message** for the following code changes. The commit message must strictly adhere to the following structure and requirements:
>
> 1. **Summary Line**
>    - Begin with a single, concise summary line (50–72 characters) that clearly expresses the primary intent and impact of the commit.
>
> 2. **Comprehensive Commit Body**
>    - Provide a multi-paragraph, deeply detailed explanation that covers:
>      - **What:**  
>        - List all changes, additions, removals, refactors, and fixes at every level (UI, backend, database, infrastructure, tests, configuration, documentation, code style).
>        - Highlight both major and minor changes, including variable renames, comments, and formatting.
>      - **Why:**  
>        - For each change, explain the motivation, problem solved, or business/user value delivered.
>        - Discuss technical debt addressed, performance/security/usability/maintainability improvements, and any prior issues or requests.
>      - **How:**  
>        - Break down the implementation approach for each major change.
>        - Explain the rationale for design patterns, algorithms, libraries, or frameworks chosen.
>        - Summarize any alternatives considered, and why they were not selected.
>        - Describe changes to control flow, data models, APIs, schemas, configuration, and external integrations.
>      - **Scope & Impact:**  
>        - Explicitly list all affected modules, components, features, workflows, and user interactions.
>        - Document any ripple effects, breaking changes, or updates to dependencies/build/CI environments.
>        - Note any changes to external/public interfaces, schemas, or protocols.
>      - **Testing:**  
>        - Describe all testing performed (manual, automated, unit, integration, performance, security).
>        - List new or updated test cases, test coverage, and outcomes.
>        - Provide steps to reproduce, verify, or validate the change.
>        - Mention known issues, limitations, or test gaps.
>      - **Migration/Deployment:**  
>        - Specify any migration steps, data transformations, or deployment notes.
>        - Describe rollback or recovery procedures, if applicable.
>        - Offer guidance for QA, reviewers, and future maintainers.
>      - **References/Traceability:**  
>        - Link to all relevant issues, tickets, pull requests, specs, discussions, and external documentation.
>      - **TODOs/Follow-ups:**  
>        - List outstanding tasks, technical debt, or further work required.
>        - Flag areas for additional investigation or improvement.
>
> 3. **Affected Entities Table**
>    - Provide a Markdown table enumerating all files, classes, functions, methods, and configuration keys affected. For each, note:
>      - The specific change (add, edit, remove, rename, refactor, etc.)
>      - The purpose of the change
>      - Any cross-references to related code or documentation
>
> 4. **Rich Markdown Formatting**
>    - Structure the message with section headings (`##`, `###`), bullet points, numbered lists, and code blocks.
>    - Include before/after code snippets or configuration diffs for all non-trivial changes, clearly annotated.
>    - Use tables to summarize changes, test results, or migration steps where appropriate.
>
> 5. **Changelog Summary**
>    - Conclude with a section explicitly summarizing everything added, changed, fixed, removed, or deprecated, using a clear changelog format.
>
> 6. **Sign-off (if required)**
>    - End with a sign-off line (e.g., `Signed-off-by: [author] <email>`) if your project policy requires it.
>
> ---
>
> **Input Diff:**  
> ```
> [PASTE YOUR DIFF HERE]
> ```
>
> ---
>
> **Guidance:**  
> - Be maximally exhaustive, precise, and unambiguous.
> - Assume the reader is technical and familiar with the codebase, but capture every nuance, decision, and rationale for future-proofing.
> - Follow and exceed best practices for conventional commits and changelogs.
> - Over-document rather than under-document to maximize clarity, traceability, and maintainability.

---

**How to use:**  
- Copy this prompt into your AI tool, replacing `[PASTE YOUR DIFF HERE]` with your actual code diff or a summary of your changes.
