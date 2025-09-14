##

1. **Users**
	-id: serial (PK, NOT NULL, UNIQUE)
	-name: varchar(100) (NOT NULL)
	-email: varchar(255) (NOT NULL, UNIQUE)
	-password: varchar(255) (NOT NULL)

	OtO:
		-UserProfiles;
		
	OtM:
		-Tasks;
		-TaskComments;
		-Logs;
		-UserRoles;
		-Notifications.


2. **Roles**
	-id: serial (PK, NOT NULL, UNIQUE)
	-name: varchar(100) (NOT NULL, UNIQUE)

	OtM:
		-UserRoles.
	
	
3. **UserRoles**
	-user_id: serial (NOT NULL, FK -> Users(id))
	-role_id: serial (NOT NULL, FK -> Roles(id))
	-project_id: serial (NOT NULL, FK -> Projects(id))
	

4. **UserProfiles**
	-user_id: serial (NOT NULL, FK -> Users(id), UNIQUE)
	-phone: varchar(20) (NOT NULL, UNIQUE)
	-address: varchar(255) (NOT NULL)
	-date_of_birth: date (NOT NULL)
	-profile_picture: text

	OtO:
		-Users.
	

5. **Projects**
	-id: serial (PK, NOT NULL, UNIQUE)
	-title: varchar(255) (NOT NULL)
	-description: text
	-start_date: date (NOT NULL)
	-end_date: date
	
	
6. **Tasks**
	-id: serial (PK, NOT NULL, UNIQUE)
	-title: varchar(255) (NOT NULL)
	-description: text
	-status: serial (NOT NULL, FK -> TaskStatuses(id))
	-creation_date: date (NOT NULL)
	-completion_date: date
	-project_id: serial (NOT NULL, FK -> Projects(id))
	-executor_id: serial (NOT NULL, FK -> Users(id))
	
	MtO:
		-Projects;
		-Users;
		-TaskStatuses.
		
	OtM:
		-TaskComments
	
	
7. **TaskStatuses**
	-id: serial (PK, NOT NULL, UNIQUE)
	-name: varchar(50) (NOT NULL, UNIQUE)
	
	OtM:
		-Tasks.


8. **TaskComments**
	-id: serial (PK, NOT NULL, UNIQUE)
	-content: text (NOT NULL)
	-creation_date: date (NOT NULL)
	-task_id: serial (NOT NULL, FK -> Tasks(id))
	-author_id: serial (NOT NULL, FK -> Users(id))
	
	MtO:
		-Tasks;
		-Users.
	
	
9. **ProjectRecources**
	-id: serial (PK, NOT NULL, UNIQUE)
	-description: text
	-type: varchar(50) (NOT NULL)
	-project_id: serial (NOT NULL, FK -> Projects(id))
	
	MtO:
		Projects.
	
	
10. **Logs**
	-id: serial (PK, NOT NULL, UNIQUE)
	-action: text (NOT NULL)
	-date: timestamp (NOT NULL)
	-user_id: serial (NOT NULL, FK -> Users(id))
	
	MtO:
		-Users.
	
	
11. **Notifications**
	-id: serial (PK, NOT NULL, UNIQUE)
	-message: text (NOT NULL)
	-time: timestamp (NOT NULL)
	-user_id: serial (NULLABLE, FK-> Users(id))
	-project_id: serial (NULLABLE, FK-> Projects(id))
	
	MtO:
		-Users;
		-Projects.