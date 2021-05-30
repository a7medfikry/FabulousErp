﻿namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChanUToC : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_sales_GS", "Show_cost_price", c => c.Boolean(nullable: false));
            DropColumn("dbo.Inv_sales_GS", "Show_unit_price");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_sales_GS", "Show_unit_price", c => c.Boolean(nullable: false));
            DropColumn("dbo.Inv_sales_GS", "Show_cost_price");
        }
    }
}
