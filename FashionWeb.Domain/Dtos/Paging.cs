using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace FashionWeb.Domain.Dtos
{
	public class Paging
	{
		public int TotalPages { get; private set; }
		public int CurrentPage { get; private set; }

		public Paging(int currentPage, int pageSize, int totalItems)
		{
			this.CurrentPage = currentPage;
			this.TotalPages = GetTotalPages(pageSize, totalItems);
		}

		private int GetTotalPages(int pageSize, int totalItems)
		{
			var totalPages = (double)totalItems / pageSize; 
			return (int)Math.Ceiling(totalPages);
		}
	}
}