﻿namespace HM.Data.Database
{
    public class DbFieldInfo
    {
        public DbFieldMode Mode { get; set; }
        public string PropertyName { get; init; } = string.Empty;
        public string ColumnName { get; init; } = string.Empty;
    }
}