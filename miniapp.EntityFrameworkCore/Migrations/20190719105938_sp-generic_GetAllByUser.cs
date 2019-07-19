using Microsoft.EntityFrameworkCore.Migrations;

namespace miniapp.EntityFrameworkCore.Migrations
{
    public partial class spgeneric_GetAllByUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"
-- =============================================
-- Author:		Niju
-- Create date: 
-- Description:	
-- =============================================
-- generic_GetAllByUser ''14eafe27-71b7-487f-8e07-67c312ea4777'', ''todo''	
-- =============================================
CREATE PROCEDURE [dbo].[generic_GetAllByUser] 
	-- Add the parameters for the stored procedure here
	@userId nvarchar(450), 
	@tableName nvarchar(400)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	

	EXECUTE('SELECT * FROM ' + @tableName  + 's WHERE CreatedById = ''' + @userId + ''' AND ModifiedById = ''' + @userId + '''')
END
GO
";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
