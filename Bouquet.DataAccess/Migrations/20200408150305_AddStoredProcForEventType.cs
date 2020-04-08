using Microsoft.EntityFrameworkCore.Migrations;

namespace Bouquet.DataAccess.Migrations
{
    public partial class AddStoredProcForEventType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROC usp_GetEventTypes 
                                    AS 
                                    BEGIN 
                                     SELECT * FROM dbo.EventTypes 
                                    END");

            migrationBuilder.Sql(@"CREATE PROC usp_GetEventType 
                                    @Id int 
                                    AS 
                                    BEGIN 
                                     SELECT * FROM dbo.EventTypes  WHERE  (Id = @Id) 
                                    END ");

            migrationBuilder.Sql(@"CREATE PROC usp_UpdateEventType
	                                @Id int,
	                                @Name varchar(30)
                                    AS 
                                    BEGIN 
                                     UPDATE dbo.EventTypes
                                     SET  Name = @Name
                                     WHERE  Id = @Id
                                    END");

            migrationBuilder.Sql(@"CREATE PROC usp_DeleteEventType
	                                @Id int
                                    AS 
                                    BEGIN 
                                     DELETE FROM dbo.EventTypes
                                     WHERE  Id = @Id
                                    END");

            migrationBuilder.Sql(@"CREATE PROC usp_CreateEventType
                                   @Name varchar(30)
                                   AS 
                                   BEGIN 
                                    INSERT INTO dbo.EventTypes(Name)
                                    VALUES (@Name)
                                   END");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE usp_GetEventTypes");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_GetEventType");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_UpdateEventType");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_DeleteEventType");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_CreateEventType");
        }
    }
}
