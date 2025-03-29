# Blogging API Project

## Project Description

The **Blogging API** project is a robust backend solution designed to allow users to create and manage blog posts, interact with other users through comments, and follow their favorite bloggers. This API is scalable, secure, and designed to serve as the backend for various front-end applications, including web, mobile, and desktop platforms.

### Key Features

- **User Authentication & Registration**

  - Users can sign up, log in, and manage their profile.
  - Supports multi-factor authentication (MFA) for enhanced security.

- **Blog Post Management**

  - Create, update, delete, and read blog posts.
  - Rich text formatting support for engaging content creation.
  - Draft and publish functionality.

- **Comment System**

  - Users can comment on blog posts, edit or delete their comments.

- **Follow System**

  - Follow and unfollow bloggers.
  - View a list of followers and followed users.

## API Endpoints

### **User Management**

| Endpoint   | Method | Description                   |
| ---------- | ------ | ----------------------------- |
| `/signup`  | POST   | Register a new user           |
| `/login`   | POST   | Authenticate a user           |
| `/profile` | GET    | Retrieve user profile details |
| `/profile` | PUT    | Update user profile           |

### **Blog Post Management**

| Endpoint      | Method | Description                   |
| ------------- | ------ | ----------------------------- |
| `/posts`      | POST   | Create a new blog post        |
| `/posts`      | GET    | Retrieve all blog posts       |
| `/posts/{id}` | GET    | Retrieve a specific blog post |
| `/posts/{id}` | PUT    | Update a blog post by ID      |
| `/posts/{id}` | DELETE | Delete a blog post by ID      |

### **Comment System**

| Endpoint               | Method | Description                       |
| ---------------------- | ------ | --------------------------------- |
| `/posts/{id}/comments` | POST   | Add a comment to a blog post      |
| `/posts/{id}/comments` | GET    | Retrieve comments for a blog post |
| `/comments/{id}`       | DELETE | Delete a comment by ID            |

### **Follow System**

| Endpoint             | Method | Description                                       |
| -------------------- | ------ | ------------------------------------------------- |
| `/follow/{userId}`   | POST   | Follow a user                                     |
| `/unfollow/{userId}` | DELETE | Unfollow a user                                   |
| `/followers`         | GET    | Retrieve followers of the authenticated user      |
| `/following`         | GET    | Retrieve users followed by the authenticated user |

---

## Functional Requirements

1. **User Management**

   - Sign up, log in, and update profile information.

2. **Blog Post Management**

   - CRUD operations for blog posts with support for rich text, tags, and images.

3. **Comments**

   - Users can add, edit, and delete comments on posts.

4. **Follow System**
   - Follow/unfollow other bloggers and view follower/following lists.

---

## Non-Functional Requirements

- **Scalability:** Efficiently handle a growing number of users and interactions.
- **Performance:** Fast response time and optimized queries.
- **Security:** Secure authentication (JWT), password hashing, and protection against SQL injection and XSS attacks.
- **Reliability:** High availability and fault tolerance.
- **Usability:** Easy-to-use API with comprehensive documentation.

---

## Technical Stack

- **Backend Language:** ASP.NET Core
- **Database:** SQL Server (with Entity Framework Core)
- **Authentication:** JWT (JSON Web Token)
- **API Documentation:** Scalar (OpenAPI)


