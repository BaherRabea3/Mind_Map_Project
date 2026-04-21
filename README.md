# MindMapManager API

A RESTful Web API built with ASP.NET Core for managing learning roadmaps, tracks, topics, resources, and community interactions.

---

## Table of Contents

- [Authentication](#authentication)
- [Endpoints](#endpoints)
  - [Account](#account)
  - [Tracks](#tracks)
  - [Roadmaps](#roadmaps)
  - [Topics](#topics)
  - [Levels](#levels)
  - [Resources](#resources)
  - [Reviews](#reviews)
  - [Bookmarks](#bookmarks)
  - [Certificates](#certificates)
  - [Community (Comments)](#community-comments)
  - [Notifications](#notifications)
  - [Search](#search)
  - [Admin](#admin)
- [Authorization Roles](#authorization-roles)
- [Error Handling](#error-handling)

---

## Authentication

The API uses **JWT Bearer Tokens** with refresh token support.

Include the token in the `Authorization` header:

```
Authorization: Bearer <your_token>
```

---

## Endpoints

### Account

Base: `/api/account`

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/register` | Public | Register a new user |
| POST | `/login` | Public | Login and receive JWT token |
| GET | `/logout` | 🔒 Required | Logout current user |
| POST | `/generate-new-jwt-token` | Public | Refresh JWT using refresh token |
| POST | `/forgot-password` | Public | Request a password reset email |
| POST | `/reset-password` | Public | Reset password using token |
| GET | `/is-email-already-registered?email=` | Public | Check if email is taken |

**Register Request Body:**
```json
{
  "name": "string",
  "email": "string",
  "password": "string"
}
```

**Login Request Body:**
```json
{
  "email": "string",
  "password": "string",
  "rememberMe": true
}
```

**Refresh Token Request Body:**
```json
{
  "token": "string",
  "refreshToken": "string"
}
```

---

### Tracks

Base: `/api/track`

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/` | Public | Get all tracks (paginated) |
| GET | `/Featured-tracks?amount=4` | Public | Get most-enrolled tracks |
| GET | `/{id}` | Public | Get roadmaps by track ID |
| POST | `/add` | 🔒 Admin | Add a new track |
| PUT | `/{id}` | 🔒 Admin | Update a track |
| DELETE | `/{id}` | 🔒 Admin | Delete a track |

**Query Parameters (GET /):**

| Param | Default | Description |
|-------|---------|-------------|
| `page` | 1 | Page number |
| `pageSize` | 6 | Items per page |
| `searchTirm` | null | Search keyword |

---

### Roadmaps

Base: `/api/roadmap`

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/` | Public | Get all roadmaps (paginated) |
| GET | `/{id}` | Public | Get roadmap details |
| POST | `/` | 🔒 Admin | Add a new roadmap |
| PUT | `/{id}` | 🔒 Admin | Update a roadmap |
| DELETE | `/{id}` | 🔒 Admin | Delete a roadmap |

**Query Parameters (GET /):**

| Param | Default | Description |
|-------|---------|-------------|
| `page` | 1 | Page number |
| `pageSize` | 6 | Items per page |
| `searchTirm` | null | Search keyword |

---

### Topics

Base: `/api/topic`

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/{id}` | Public | Get topic with details |
| POST | `/` | 🔒 Admin | Add a new topic |
| PUT | `/{id}` | 🔒 Admin | Update a topic |
| DELETE | `/{id}` | 🔒 Admin | Delete a topic |

---

### Levels

Base: `/api/level`

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/{id}` | Public | Get level with details |
| POST | `/` | 🔒 Admin | Add a new level |
| PUT | `/{id}` | 🔒 Admin | Update a level |
| DELETE | `/{id}` | 🔒 Admin | Delete a level |

---

### Resources

Base: `/api/resource`

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/{id}` | Public | Get resource with details |
| POST | `/` | 🔒 Admin | Add a new resource |
| PUT | `/{id}` | 🔒 Admin | Update a resource |
| DELETE | `/{id}` | 🔒 Admin | Delete a resource |

---

### Reviews

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/roadmaps/{id}/reviews` | Public | Get reviews for a roadmap |
| POST | `/api/roadmaps/{id}/reviews` | 🔒 Required | Add a review |
| DELETE | `/api/reviews/{id}` | 🔒 Required | Delete a review (own or Admin) |

**Query Parameters (GET reviews):**

| Param | Default | Description |
|-------|---------|-------------|
| `page` | 1 | Page number |
| `pageSize` | 10 | Items per page |

**Add Review Request Body:**
```json
{
  "content": "string",
  "rate": 5
}
```

---

### Bookmarks

Base: `/api/bookmarks`

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/` | 🔒 Required | Get my bookmarks (paginated) |
| POST | `/{resourceId}` | 🔒 Required | Add a bookmark |
| DELETE | `/{resourceId}` | 🔒 Required | Remove a bookmark |

**Query Parameters (GET /):**

| Param | Default | Description |
|-------|---------|-------------|
| `page` | 1 | Page number |
| `pageSize` | 10 | Items per page |

**Response (GET /):**
```json
{
  "items": [
    {
      "resId": 0,
      "name": "string",
      "type": "string",
      "resUrl": "string",
      "paid": false
    }
  ],
  "totalCount": 0,
  "page": 1,
  "pageSize": 10,
  "totalPages": 0,
  "hasNext": false,
  "hasPrevious": false
}
```

---

### Certificates

Base: `/api/certificates`

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/` | 🔒 Required | Get my certificates |
| GET | `/{id}` | 🔒 Required | Get certificate by ID |

**Response:**
```json
{
  "certId": 0,
  "certUrl": "string",
  "issuedAt": "datetime",
  "roadmapId": 0
}
```

---

### Community (Comments)

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/topics/{id}/comments` | 🔒 Required | Get comments on a topic |
| POST | `/api/topics/{id}/comments` | 🔒 Required | Add a comment to a topic |
| POST | `/api/comments/{id}/reply` | 🔒 Required | Reply to a comment |
| DELETE | `/api/comments/{id}` | 🔒 Required | Delete own comment |
| DELETE | `/api/admin/comments/{id}` | 🔒 Admin | Admin delete any comment |

> ⚠️ User must be enrolled in the track to access community features. Returns `403 Forbidden` if not enrolled.

**Add Comment / Reply Request Body:**
```json
{
  "content": "string"
}
```

**Response (GET comments):**
```json
[
  {
    "comId": 0,
    "content": "string",
    "createdAt": "datetime",
    "userId": 0,
    "replies": [
      {
        "comId": 0,
        "content": "string",
        "createdAt": "datetime",
        "userId": 0,
        "replies": []
      }
    ]
  }
]
```

---

### Notifications

Base: `/api/notifications`

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/` | 🔒 Required | Get my notifications (paginated) |
| GET | `/unread-count` | 🔒 Required | Get unread notification count |
| PUT | `/{id}/read` | 🔒 Required | Mark a notification as read |
| PUT | `/read-all` | 🔒 Required | Mark all notifications as read |

**Query Parameters (GET /):**

| Param | Default | Description |
|-------|---------|-------------|
| `page` | 1 | Page number |
| `pageSize` | 10 | Items per page |

**Response (GET /):**
```json
{
  "items": [
    {
      "notId": 0,
      "message": "string",
      "read": false,
      "createdAt": "datetime",
      "refType": "string",
      "refId": 0
    }
  ],
  "totalCount": 0,
  "page": 1,
  "pageSize": 10,
  "totalPages": 0,
  "hasNext": false,
  "hasPrevious": false
}
```

---

### Search

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/search` | Public | Search across all content |

**Query Parameters:**

| Param | Required | Default | Description |
|-------|----------|---------|-------------|
| `q` | ✅ Yes | - | Search keyword |
| `page` | No | 1 | Page number |
| `pageSize` | No | 10 | Items per page |

**Response:**
```json
{
  "tracks": [
    { "trackId": 0, "name": "string", "description": "string" }
  ],
  "roadmaps": [
    { "rid": 0, "name": "string", "description": "string" }
  ],
  "topics": [
    { "topicId": 0, "name": "string" }
  ]
}
```

---

### Admin

Base: `/api/admin` — requires **Admin** role.

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/users` | Get all users (filterable + paginated) |
| GET | `/users/{id}` | Get user by ID |
| PUT | `/users/{id}/status` | Change user status |
| PUT | `/users/{id}/role` | Change user role |
| DELETE | `/users/{id}` | Delete a user |
| GET | `/dashboard` | Get dashboard stats |
| DELETE | `/comments/{id}` | Delete any comment |

**Query Parameters (GET /users):**

| Param | Default | Description |
|-------|---------|-------------|
| `status` | null | Filter by status (Active/Banned) |
| `search` | null | Search by name or email |
| `page` | 1 | Page number |
| `pageSize` | 10 | Items per page |

**Change Status Request Body:**
```json
{
  "status": "Active | Banned"
}
```

**Change Role Request Body:**
```json
{
  "role": "Admin | Member"
}
```

**Dashboard Response:**
```json
{
  "totalUsers": 0,
  "activeUsers": 0,
  "totalTracks": 0,
  "totalRoadmaps": 0,
  "totalEnrollments": 0,
  "totalCertificates": 0,
  "totalComments": 0,
  "newUsersThisWeek": 0,
  "newUsersThisMonth": 0
}
```

---

## Authorization Roles

| Role | Access Level |
|------|-------------|
| `Member` | Assigned on registration. Can bookmark, review, comment, and manage their own content. |
| `Admin` | Full access to all endpoints including user management, content CRUD, and admin dashboard. |

---

## Error Handling

The API returns standard HTTP status codes:

| Code | Meaning |
|------|---------|
| `200 OK` | Success |
| `201 Created` | Resource created |
| `204 No Content` | Operation successful, no body |
| `400 Bad Request` | Validation error or bad input |
| `401 Unauthorized` | Missing or invalid JWT token |
| `403 Forbidden` | Insufficient permissions |
| `404 Not Found` | Resource does not exist |
| `409 Conflict` | Resource already exists (e.g., duplicate email) |
| `500 Internal Server Error` | Unexpected server error |

Error responses return a plain string message or a `ProblemDetails` object describing the issue.
