# Movie Booking API – Seat Management

## Overview

This project implements the **seat availability and booking behaviour** for a movie booking system. The focus is strictly on **show creation, seat allocation, seat holding, automatic seat release, and booking confirmation**. Other features such as authentication, payments, movie listings, or UI are intentionally out of scope.

The system ensures **concurrency safety**, **temporary seat locking**, and **automatic release of unpaid seats** using background processing.

---

## Key Features

* **Show Creation with Predefined Seats**
  When a show is created, seats are automatically allocated for that show.

* **Seat Status Tracking**
  Seats can be in one of the following states:

  * Available
  * Held
  * Booked

* **Seat Holding (Temporary Locking)**
  When a user selects seats, they are marked as *Held* and become unavailable to other users.

* **Automatic Seat Release**
  If the user does not confirm the booking or cancels the booking, the held seats are automatically released and become available again.

* **Booking Confirmation**
  Once the user confirms the booking, held seats are permanently marked as *Booked*.

* **Concurrency Handling**
  Uses optimistic concurrency to prevent multiple users from booking the same seat simultaneously.

---

## API Endpoints

### 1. Create Show and Allocate Seats

**POST** `/api/shows`

* Creates a new show
* Automatically allocates predefined seats for the show

---

### 2. Get Seats by Show (Grouped by Row)

**GET** `/api/shows/{showId}`

* Retrieves all seats for a show
* Seats are grouped by row
* Displays seat number and current status (Available / Held / Booked)

---

### 3. Hold / Select Seats

**POST** `/api/seats/hold`

* Temporarily holds selected seats
* Held seats are unavailable to other users
* Hold expires automatically after 1 minute if not confirmed

---

### 4. Confirm Booking

**POST** `/api/bookings/confirm`

* Confirms the booking
* Converts held seats into booked seats
* Prevents further modification

---

## Automatic Seat Release

A **background service** runs at regular intervals to:

* Detect expired seat holds
* Release seats that were not confirmed within the allowed time
* Make those seats available for other users

This ensures system reliability and prevents seat starvation.

---

## Technology Stack

* **Backend:** ASP.NET Core Web API
* **Language:** C# (.NET)
* **Database:** Microsoft SQL Server
* **ORM:** Entity Framework Core
* **IDE:** Visual Studio

---

## Configuration Management

* Sensitive configuration values (such as server name and credentials) are **not committed to source control**.
* A **sanitized copy** of `appsettings.json` (without server-specific values) is included for reference.
* Actual environment-specific values should be configured locally or via environment variables.

---

## Git Ignore Strategy

The following are excluded from version control:

* Build artifacts (`bin`, `obj`)
* IDE-specific files (`.vs`, `.idea`, `.vscode`)
* Environment-specific configuration files
* Database and temporary files

This ensures a clean and portable repository.

---

## Scope Clarification

This project is intentionally limited to:

* Seat allocation
* Seat holding
* Seat release
* Booking confirmation

It does **not** include:

* Authentication or authorization
* Payment processing
* UI or frontend implementation
* Movie or theatre management

---

## Conclusion

This project demonstrates a robust and scalable approach to managing seat availability in a concurrent environment. It follows clean architectural principles, ensures data consistency, and is suitable as a foundation for a larger movie booking system.
