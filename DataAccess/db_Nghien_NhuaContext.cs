using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DataAccess
{
    public partial class db_Nghien_NhuaContext : DbContext
    {
        public db_Nghien_NhuaContext()
        {
        }

        public db_Nghien_NhuaContext(DbContextOptions<db_Nghien_NhuaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Earphone> Earphones { get; set; }
        public virtual DbSet<KeyBoard> KeyBoards { get; set; }
        public virtual DbSet<Keycap> Keycaps { get; set; }
        public virtual DbSet<Kit> Kits { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Mouse> Mice { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Switch> Switches { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<View> Views { get; set; }
        public virtual DbSet<Staff> Staffs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.

                optionsBuilder.UseSqlServer("Data Source=MSI;Initial Catalog=NghienNhua;Persist Security Info=True;User ID=sa;Password=123;Trust Server Certificate=True");

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccId);

                entity.ToTable("Account");

                entity.Property(e => e.AccId).HasColumnName("acc_id");

                entity.Property(e => e.AccGmail)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("acc_gmail");

                entity.Property(e => e.AccPassword)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("acc_password");

                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("role");
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("Cart");

                entity.Property(e => e.CartId).HasColumnName("cart_id");

                entity.Property(e => e.CartQuantity).HasColumnName("cart_quantity");

                entity.Property(e => e.ProId).HasColumnName("pro_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Pro)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.ProId)
                    .HasConstraintName("FK_Cart_Product");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Cart_User");
            });

            modelBuilder.Entity<Earphone>(entity =>
            {
                entity.HasKey(e => e.EarId);

                entity.ToTable("Earphone");

                entity.HasIndex(e => e.ProId, "IX_Earphone")
                    .IsUnique();

                entity.Property(e => e.EarId).HasColumnName("ear_id");

                entity.Property(e => e.EarChargingPort)
                    .HasMaxLength(255)
                    .HasColumnName("ear_charging_port");

                entity.Property(e => e.EarCompatibility)
                    .HasMaxLength(255)
                    .HasColumnName("ear_compatibility");

                entity.Property(e => e.EarConnect)
                    .HasMaxLength(255)
                    .HasColumnName("ear_connect");

                entity.Property(e => e.EarConnectTech)
                    .HasMaxLength(255)
                    .HasColumnName("ear_connect_tech");

                entity.Property(e => e.EarControl)
                    .HasMaxLength(255)
                    .HasColumnName("ear_control");

                entity.Property(e => e.EarPlug)
                    .HasMaxLength(255)
                    .HasColumnName("ear_plug");

                entity.Property(e => e.EarType)
                    .HasMaxLength(255)
                    .HasColumnName("ear_type");

                entity.Property(e => e.EarUtility)
                    .HasMaxLength(255)
                    .HasColumnName("ear_utility");

                entity.Property(e => e.EarWireLength)
                    .HasMaxLength(255)
                    .HasColumnName("ear_wire_length");

                entity.Property(e => e.ProId).HasColumnName("pro_id");

                entity.HasOne(d => d.Pro)
                    .WithOne(p => p.Earphone)
                    .HasForeignKey<Earphone>(d => d.ProId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Earphone_Product");
            });

            modelBuilder.Entity<KeyBoard>(entity =>
            {
                entity.HasKey(e => e.KbId);

                entity.ToTable("KeyBoard");

                entity.HasIndex(e => e.ProId, "IX_KeyBoard")
                    .IsUnique();

                entity.Property(e => e.KbId).HasColumnName("kb_id");

                entity.Property(e => e.KbCase)
                    .HasMaxLength(255)
                    .HasColumnName("kb_case");

                entity.Property(e => e.KbKeycap)
                    .HasMaxLength(255)
                    .HasColumnName("kb_keycap");

                entity.Property(e => e.KbLed)
                    .HasMaxLength(255)
                    .HasColumnName("kb_led");

                entity.Property(e => e.KbMode)
                    .HasMaxLength(255)
                    .HasColumnName("kb_mode");

                entity.Property(e => e.KbPlate)
                    .HasMaxLength(255)
                    .HasColumnName("kb_plate");

                entity.Property(e => e.KbSwitch)
                    .HasMaxLength(255)
                    .HasColumnName("kb_switch");

                entity.Property(e => e.ProId).HasColumnName("pro_id");

                entity.HasOne(d => d.Pro)
                    .WithOne(p => p.KeyBoard)
                    .HasForeignKey<KeyBoard>(d => d.ProId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KeyBoard_Product");
            });

            modelBuilder.Entity<Keycap>(entity =>
            {
                entity.HasKey(e => e.KcId);

                entity.ToTable("Keycap");

                entity.HasIndex(e => e.ProId, "IX_Keycap")
                    .IsUnique();

                entity.Property(e => e.KcId).HasColumnName("kc_id");

                entity.Property(e => e.KcLayout)
                    .HasMaxLength(255)
                    .HasColumnName("kc_layout");

                entity.Property(e => e.KcMaterial)
                    .HasMaxLength(255)
                    .HasColumnName("kc_material");

                entity.Property(e => e.KcReliability)
                    .HasMaxLength(255)
                    .HasColumnName("kc_reliability");

                entity.Property(e => e.KcThickness)
                    .HasMaxLength(255)
                    .HasColumnName("kc_thickness");

                entity.Property(e => e.ProId).HasColumnName("pro_id");

                entity.HasOne(d => d.Pro)
                    .WithOne(p => p.Keycap)
                    .HasForeignKey<Keycap>(d => d.ProId)
                    .HasConstraintName("FK_Keycap_Product");
            });

            modelBuilder.Entity<Kit>(entity =>
            {
                entity.ToTable("Kit");

                entity.HasIndex(e => e.ProId, "IX_Kit")
                    .IsUnique();

                entity.Property(e => e.KitId).HasColumnName("kit_id");

                entity.Property(e => e.KitCase)
                    .HasMaxLength(255)
                    .HasColumnName("kit_case");

                entity.Property(e => e.KitCircuit)
                    .HasMaxLength(255)
                    .HasColumnName("kit_circuit");

                entity.Property(e => e.KitLayout)
                    .HasMaxLength(255)
                    .HasColumnName("kit_layout");

                entity.Property(e => e.KitMode)
                    .HasMaxLength(255)
                    .HasColumnName("kit_mode");

                entity.Property(e => e.KitPlate)
                    .HasMaxLength(255)
                    .HasColumnName("kit_plate");

                entity.Property(e => e.ProId).HasColumnName("pro_id");

                entity.HasOne(d => d.Pro)
                    .WithOne(p => p.Kit)
                    .HasForeignKey<Kit>(d => d.ProId)
                    .HasConstraintName("FK_Kit_Product");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Message");

                entity.Property(e => e.ConversationId).HasColumnName("conversationID");

                entity.Property(e => e.IsFromShop)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("isFromShop");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasColumnName("text");

                entity.Property(e => e.Timetamp).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<Mouse>(entity =>
            {
                entity.ToTable("Mouse");

                entity.HasIndex(e => e.ProId, "IX_Mouse")
                    .IsUnique();

                entity.Property(e => e.MouseId).HasColumnName("mouse_id");

                entity.Property(e => e.MouseCompatibility)
                    .HasMaxLength(255)
                    .HasColumnName("mouse_compatibility");

                entity.Property(e => e.MouseDpi)
                    .HasMaxLength(255)
                    .HasColumnName("mouse_dpi");

                entity.Property(e => e.MouseLed)
                    .HasMaxLength(255)
                    .HasColumnName("mouse_led");

                entity.Property(e => e.MouseTypeBattery)
                    .HasMaxLength(255)
                    .HasColumnName("mouse_type_battery");

                entity.Property(e => e.MouseWeight)
                    .HasMaxLength(255)
                    .HasColumnName("mouse_weight");

                entity.Property(e => e.MouseWireLength)
                    .HasMaxLength(255)
                    .HasColumnName("mouse_wire_length");

                entity.Property(e => e.ProId).HasColumnName("pro_id");

                entity.HasOne(d => d.Pro)
                    .WithOne(p => p.Mouse)
                    .HasForeignKey<Mouse>(d => d.ProId)
                    .HasConstraintName("FK_Mouse_Product");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.OrderAddress)
                    .HasMaxLength(500)
                    .HasColumnName("order_address");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("date")
                    .HasColumnName("order_date");

                entity.Property(e => e.OrderEmail)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("order_email");

                entity.Property(e => e.OrderName)
                    .HasMaxLength(255)
                    .HasColumnName("order_name");

                entity.Property(e => e.OrderPhoneNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("order_phoneNumber");

                entity.Property(e => e.OrderStatus)
                    .HasMaxLength(255)
                    .HasColumnName("order_status");

                entity.Property(e => e.OrderTotalMoney)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("order_totalMoney");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Orders_User");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.OdId);

                entity.ToTable("Order_details");

                entity.Property(e => e.OdId).HasColumnName("od_id");

                entity.Property(e => e.OdPrice)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("od_price");

                entity.Property(e => e.OdQuantity).HasColumnName("od_quantity");

                entity.Property(e => e.OdTotalMoney)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("od_totalMoney");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.ProId).HasColumnName("pro_id");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_Order_details_Orders");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProId);

                entity.ToTable("Product");

                entity.Property(e => e.ProId).HasColumnName("pro_id");

                entity.Property(e => e.ProBrand)
                    .HasMaxLength(255)
                    .HasColumnName("pro_brand");

                entity.Property(e => e.ProCategory)
                    .HasMaxLength(50)
                    .HasColumnName("pro_category");

                entity.Property(e => e.ProDate)
                    .HasColumnType("date")
                    .HasColumnName("pro_date");

                entity.Property(e => e.ProDescription).HasColumnName("pro_description");

                entity.Property(e => e.ProDiscount)
                    .HasMaxLength(50)
                    .HasColumnName("pro_discount");

                entity.Property(e => e.ProImage)
                    .HasMaxLength(255)
                    .HasColumnName("pro_image");

                entity.Property(e => e.ProName)
                    .HasMaxLength(255)
                    .HasColumnName("pro_name");

                entity.Property(e => e.ProOrigin)
                    .HasMaxLength(255)
                    .HasColumnName("pro_origin");

                entity.Property(e => e.ProPrice)
                    .HasMaxLength(50)
                    .HasColumnName("pro_price");

                entity.Property(e => e.ProQuantity).HasColumnName("pro_quantity");
            });

            modelBuilder.Entity<Switch>(entity =>
            {
                entity.ToTable("Switch");

                entity.HasIndex(e => e.ProId, "IX_Switch")
                    .IsUnique();

                entity.Property(e => e.SwitchId).HasColumnName("switch_id");

                entity.Property(e => e.ProId).HasColumnName("pro_id");

                entity.Property(e => e.SwitchDepth)
                    .HasMaxLength(255)
                    .HasColumnName("switch_depth");

                entity.Property(e => e.SwitchPin)
                    .HasMaxLength(255)
                    .HasColumnName("switch_pin");

                entity.Property(e => e.SwitchReliability)
                    .HasMaxLength(255)
                    .HasColumnName("switch_reliability");

                entity.Property(e => e.SwitchSpring)
                    .HasMaxLength(255)
                    .HasColumnName("switch_spring");

                entity.Property(e => e.SwitchType)
                    .HasMaxLength(255)
                    .HasColumnName("switch_type");

                entity.HasOne(d => d.Pro)
                    .WithOne(p => p.Switch)
                    .HasForeignKey<Switch>(d => d.ProId)
                    .HasConstraintName("FK_Switch_Product");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.AccId, "IX_User")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.AccId).HasColumnName("acc_id");

                entity.Property(e => e.UserAddress)
                    .HasMaxLength(500)
                    .HasColumnName("user_address");

                entity.Property(e => e.UserFullname)
                    .HasMaxLength(255)
                    .HasColumnName("user_fullname");

                entity.Property(e => e.UserSdt)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("user_sdt");

                entity.HasOne(d => d.Acc)
                    .WithOne(p => p.User)
                    .HasForeignKey<User>(d => d.AccId)
                    .HasConstraintName("FK_User_Account");
            });

            modelBuilder.Entity<View>(entity =>
            {
                entity.HasKey(e => e.Date);

                entity.ToTable("View");

                entity.Property(e => e.Date).HasMaxLength(50);
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.ToTable("Staff");

                entity.HasIndex(e => e.AccId, "IX_Staff")
                    .IsUnique();

                entity.Property(e => e.StaffId).HasColumnName("staff_id");

                entity.Property(e => e.AccId).HasColumnName("acc_id");

                entity.Property(e => e.SftaffDayOut)
                    .HasColumnType("date")
                    .HasColumnName("sftaff_dayOut");

                entity.Property(e => e.StaffAddress)
                    .HasMaxLength(500)
                    .HasColumnName("staff_address");

                entity.Property(e => e.StaffCitizenIdentityNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("staff_citizenIdentityNumber");

                entity.Property(e => e.StaffDateOfBirth)
                    .HasColumnType("date")
                    .HasColumnName("staff_dateOfBirth");

                entity.Property(e => e.StaffDayJoin)
                    .HasColumnType("date")
                    .HasColumnName("staff_dayJoin");

                entity.Property(e => e.StaffFullname)
                    .HasMaxLength(255)
                    .HasColumnName("staff_fullname");

                entity.Property(e => e.StaffGender)
                    .HasMaxLength(50)
                    .HasColumnName("staff_gender");

                entity.Property(e => e.StaffPhoneNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("staff_phoneNumber");

                entity.Property(e => e.StaffSalary)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("staff_salary");

                entity.Property(e => e.StaffStatus)
                    .HasMaxLength(50)
                    .HasColumnName("staff_status");

                entity.HasOne(d => d.Acc)
                    .WithOne(p => p.staff)
                    .HasForeignKey<Staff>(d => d.AccId)
                    .HasConstraintName("FK_Staff_Account");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
