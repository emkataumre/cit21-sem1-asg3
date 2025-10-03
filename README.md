# CIT Assignment 3 – Network Service

---

## Part I – Supporting Classes

### 1. CategoryService

Provides full CRUD functionality over an in-memory list of categories.

**Interface:**

- `public List<Category> GetCategories();`
- `public Category? GetCategory(int cid);`
- `public bool UpdateCategory(int id, string newName);`
- `public bool DeleteCategory(int id);`
- `public bool CreateCategory(int id, string name);`

---

### 2. RequestValidator

Responsible for validating incoming request objects against CJTP requirements.

**Method:**

- `public Response ValidateRequest(Request request);`

**Validation checks:**

- All required fields present (`method`, `path`, `date`).
- `method` must be one of: `create`, `read`, `update`, `delete`, `echo`.
- `date` must be a valid Unix timestamp (64-bit integer).
- `body` present and correctly structured if required:
  - For `create` and `update`: must be valid JSON object.
  - For `echo`: must be plain text.

If valid → return `"1 Ok"`.  
If invalid → return `"4 Bad Request"` with reasons.

---

### 3. URL Parsing Utility

Parses and interprets request paths, extracting identifiers when present.

**Properties:**

- `public bool HasId { get; set; }`
- `public int Id { get; set; }`
- `public string Path { get; set; }`

**Method:**

- `public bool ParseUrl(string url);`

**Behavior:**

- Returns `true` if path is valid, otherwise `false`.
- Example:  
  Input: `/api/categories/1`  
  Output: `Path = "/api/categories"`, `HasId = true`, `Id = 1`

---

## Part II – Web Service Implementation

### 1. Server Requirements

- Implement using **TcpListener** and **TcpClient**.
- Server must listen on **port 50004**.
- Handle multiple client connections (multithreading recommended).
- Must accept client connections, receive JSON requests, and send JSON responses.
- Ignore invalid or malformed connections (bad JSON, missing fields, empty requests).

### 2. Protocol Compliance

- Fully implement CJTP:
  - Validate structure and content of incoming requests.
  - Respond with correct status codes and error messages.
  - Execute CRUD operations on category data if valid.

**Request Object Structure:**

```json
{
  "method": "<METHOD>",
  "path": "<PATH>",
  "date": <DATE>,
  "body": <BODY>
}
```
