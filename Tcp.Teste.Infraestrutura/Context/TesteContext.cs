using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tcp.Teste.Infraestrutura.Context
{
	public class TesteContext : DbContext
	{
		public string DbPath { get; }

		public TesteContext()
		{
			var path = Environment.CurrentDirectory;
			DbPath = System.IO.Path.Join(path, "teste.db");
		}

		// The following configures EF to create a Sqlite database file in the
		// special "local" folder for your platform.
		protected override void OnConfiguring(DbContextOptionsBuilder options)
			=> options.UseSqlite($"Data Source={DbPath}");
	}
}
