//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project_64132985.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.CartDetails = new HashSet<CartDetail>();
            this.OrderCustomerDetails = new HashSet<OrderCustomerDetail>();
        }
    
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string Descr { get; set; }
        public int Price { get; set; }
        public string Category { get; set; }
        public string Img { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public string Colors { get; set; }
        public string Sizes { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CartDetail> CartDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderCustomerDetail> OrderCustomerDetails { get; set; }
    }
}
