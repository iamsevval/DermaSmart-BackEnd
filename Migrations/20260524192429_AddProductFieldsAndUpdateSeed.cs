using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DermaSmart.API.Migrations
{
    /// <inheritdoc />
    public partial class AddProductFieldsAndUpdateSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActiveIngredients",
                table: "products",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "products",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "products",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "UsagePurpose",
                table: "products",
                type: "TEXT",
                nullable: false,
                defaultValue: "");



            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Price", "ActiveIngredients", "UsagePurpose", "ImageUrl" },
                values: new object[] { 150.00m, "Salisilik Asit, Gliserin", "Günlük yüz temizliği, fazla yağı arındırma", "https://images.unsplash.com/photo-1556228578-0d85b1a4d571?auto=format&fit=crop&w=800&q=80" });
            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Price", "ActiveIngredients", "UsagePurpose", "ImageUrl" },
                values: new object[] { 350.00m, "C Vitamini, Ferulik Asit", "Cilt tonu eşitleme, aydınlatma, antioksidan koruma", "https://images.unsplash.com/photo-1617897903246-719242758050?auto=format&fit=crop&w=800&q=80" });
            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Price", "ActiveIngredients", "UsagePurpose", "ImageUrl" },
                values: new object[] { 280.00m, "Niasinamid (B3 Vitamini), Çinko", "Gözenek sıkılaştırma, sebum dengeleme, kızarıklık giderme", "https://images.unsplash.com/photo-1620916566398-39f1143ab7be?auto=format&fit=crop&w=800&q=80" });
            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Price", "ActiveIngredients", "UsagePurpose", "ImageUrl" },
                values: new object[] { 420.00m, "BHA (Salisilik Asit)", "Siyah nokta temizleme, sivilce oluşumunu engelleme", "https://images.unsplash.com/photo-1556228720-192a6af4e662?auto=format&fit=crop&w=800&q=80" });
            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Price", "ActiveIngredients", "UsagePurpose", "ImageUrl" },
                values: new object[] { 550.00m, "Retinol, Peptitler, Hiyalüronik Asit", "İnce çizgi ve kırışıklık görünümünü azaltma, hücre yenileme", "https://images.unsplash.com/photo-1598440947619-2ce6fff7ce05?auto=format&fit=crop&w=800&q=80" });
            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Price", "ActiveIngredients", "UsagePurpose", "ImageUrl" },
                values: new object[] { 380.00m, "Glikolik Asit (AHA), Laktik Asit", "Cilt yüzeyini yenileme, ölü hücreleri uzaklaştırma", "https://images.unsplash.com/photo-1608248543803-ba4f8c70ae0b?auto=format&fit=crop&w=800&q=80" });
            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Price", "ActiveIngredients", "UsagePurpose", "ImageUrl" },
                values: new object[] { 310.00m, "Seramidler, Kolesterol, Yağ Asitleri", "Cilt bariyerini güçlendirme, yoğun nem sağlama", "https://images.unsplash.com/photo-1601049541289-9b1b7ceb4c4c?auto=format&fit=crop&w=800&q=80" });
            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Price", "ActiveIngredients", "UsagePurpose", "ImageUrl" },
                values: new object[] { 480.00m, "Skualen, Niasinamid, Pantenol", "Gece boyunca cildi onarma, besleme ve yenileme", "https://images.unsplash.com/photo-1615397323753-b09e200c920e?auto=format&fit=crop&w=800&q=80" });
            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Price", "ActiveIngredients", "UsagePurpose", "ImageUrl" },
                values: new object[] { 290.00m, "Çinko Oksit, Titanyum Dioksit, E Vitamini", "UVA ve UVB ışınlarına karşı günlük koruma", "https://images.unsplash.com/photo-1629198728070-0708cb21422b?auto=format&fit=crop&w=800&q=80" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "ActiveIngredients",
                table: "products");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "products");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "products");

            migrationBuilder.DropColumn(
                name: "UsagePurpose",
                table: "products");
        }
    }
}
