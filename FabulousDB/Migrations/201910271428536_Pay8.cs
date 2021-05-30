namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay8 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Assign_payable_doc", name: "Vendor Id", newName: "Vendor_id");
            RenameColumn(table: "dbo.Assign_payable_doc", name: "Document type", newName: "Doc_type");
            RenameColumn(table: "dbo.Assign_payable_doc", name: "Applay Date", newName: "Applay_date");
            RenameColumn(table: "dbo.Assign_payable_doc", name: "Orginal Amount", newName: "Orginal_amount");
            RenameColumn(table: "dbo.Assign_payable_doc", name: "Applay Assign", newName: "Applay_assign");
            RenameColumn(table: "dbo.Assign_payable_doc", name: "Unassign amount", newName: "Unassign_amount");
            RenameIndex(table: "dbo.Assign_payable_doc", name: "IX_Vendor Id", newName: "IX_Vendor_id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Assign_payable_doc", name: "IX_Vendor_id", newName: "IX_Vendor Id");
            RenameColumn(table: "dbo.Assign_payable_doc", name: "Unassign_amount", newName: "Unassign amount");
            RenameColumn(table: "dbo.Assign_payable_doc", name: "Applay_assign", newName: "Applay Assign");
            RenameColumn(table: "dbo.Assign_payable_doc", name: "Orginal_amount", newName: "Orginal Amount");
            RenameColumn(table: "dbo.Assign_payable_doc", name: "Applay_date", newName: "Applay Date");
            RenameColumn(table: "dbo.Assign_payable_doc", name: "Doc_type", newName: "Document type");
            RenameColumn(table: "dbo.Assign_payable_doc", name: "Vendor_id", newName: "Vendor Id");
        }
    }
}
