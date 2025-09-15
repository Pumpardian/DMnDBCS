1. **Users**
	- **id**: serial (PK, NOT NULL, UNIQUE)
	- **name**: varchar(100) (NOT NULL)
	- **email**: varchar(255) (NOT NULL, UNIQUE)
	- **password**: varchar(255) (NOT NULL)

	**OtO**:
		- **UserProfiles**;
		
	**OtM**:
		- **Tasks**;
		- **TaskComments**;
		- **Logs**;
		- **UserRoles**;
		- **Notifications**.

	Сущность пользователя, по сути являющаяся аккаунтом, дополняется с помощью UserProfile.

2. **Roles**
	- **id**: serial (PK, NOT NULL, UNIQUE)
	- **name**: varchar(100) (NOT NULL, UNIQUE)

	**OtM**:
		- **UserRoles**.
		
	Сущность роли для создания иерархии при выполнении проектов и задач.
	
	
3. **UserRoles**
	- **user_id**: serial (NOT NULL, FK -> Users(id))
	- **role_id**: serial (NOT NULL, FK -> Roles(id))
	- **project_id**: serial (NOT NULL, FK -> Projects(id))
	
	**MtO**
		- **Users**;
		- **Roles**;
		- **Projects**.
	
	Сущность которая отвечает за роль пользователя в конкретном проекте.
	

4. **UserProfiles**
	- **user_id**: serial (NOT NULL, FK -> Users(id), UNIQUE)
	- **phone**: varchar(20) (NOT NULL, UNIQUE)
	- **address**: varchar(255) (NOT NULL)
	- **date_of_birth**: date (NOT NULL)
	- **profile_picture**: text

	**OtO**:
		- **Users**.
		
	Сущность отвечающая за данные конкретного пользователя и закреплена за ним.
	

5. **Projects**
	- **id**: serial (PK, NOT NULL, UNIQUE)
	- **title**: varchar(255) (NOT NULL)
	- **description**: text
	- **start_date**: date (NOT NULL)
	- **end_date**: date
	
	**OtM**
		- **Tasks**;
		- **Notifications**.
	
	Сущность представляющая проект, имеет дату начала (обязательно) и дату конца после окончания.
	
	
6. **Tasks**
	- **id**: serial (PK, NOT NULL, UNIQUE)
	- **title**: varchar(255) (NOT NULL)
	- **description**: text
	- **status**: serial (NOT NULL, FK -> TaskStatuses(id))
	- **creation_date**: date (NOT NULL)
	- **completion_date**: date
	- **project_id**: serial (NOT NULL, FK -> Projects(id))
	- **executor_id**: serial (NOT NULL, FK -> Users(id))
	
	**MtO**:
		- **Projects**;
		- **Users**;
		- **TaskStatuses**.
		
	**OtM**:
		- **TaskComments**.
	
	Сущность дополняющая сущность проектов, по сути для одного проекта может существовать множество подзадач.
	
	
7. **TaskStatuses**
	- **id**: serial (PK, NOT NULL, UNIQUE)
	- **name**: varchar(50) (NOT NULL, UNIQUE)
	
	**OtM**:
		- **Tasks**.

	Сущность представляющая статус задачи, в основном "в процессе", "приостановлена" и т.д.


8. **TaskComments**
	- **id**: serial (PK, NOT NULL, UNIQUE)
	- **content**: text (NOT NULL)
	- **creation_date**: date (NOT NULL)
	- **task_id**: serial (NOT NULL, FK -> Tasks(id))
	- **author_id**: serial (NOT NULL, FK -> Users(id))
	
	**MtO**:
		- **Tasks**;
		- **Users**.
		
	Сущность комментариев к задачам, чтобы исполнители могли прокомментировать какие-нибудь ньюансы, а вышестоящие по иерархии могли отвечать на заданные вопросы.
	
	
9. **ProjectRecources**
	- **id**: serial (PK, NOT NULL, UNIQUE)
	- **description**: text
	- **type**: varchar(50) (NOT NULL)
	- **project_id**: serial (NOT NULL, FK -> Projects(id))
	
	**MtO**:
		- **Projects**.
		
	Сущность ресурсов, выделяемых под конкретный проект, соответсвенно и под поставленные в рамках этого проекта задачи.
	
	
10. **Logs**
	- **id**: serial (PK, NOT NULL, UNIQUE)
	- **action**: text (NOT NULL)
	- **date**: timestamp (NOT NULL)
	- **user_id**: serial (NOT NULL, FK -> Users(id))
	
	**MtO**:
		- **Users**.
		
	Сущность отвечающая за логирование различных действий для восстановления их последовательности, что может быть важно при принятии определенных решений.
	
	
11. **Notifications**
	- **id**: serial (PK, NOT NULL, UNIQUE)
	- **message**: text (NOT NULL)
	- **time**: timestamp (NOT NULL)
	- **user_id**: serial (NULLABLE, FK-> Users(id))
	- **project_id**: serial (NULLABLE, FK-> Projects(id))
	
	**MtO**:
		- **Users**;
		- **Projects**.
		
	Сущность уведомлений по поводу проекта, когда необходимо уведомить о изменении планов и т.д.