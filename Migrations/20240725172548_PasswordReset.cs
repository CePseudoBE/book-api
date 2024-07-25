using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BookApi.Migrations
{
    /// <inheritdoc />
    public partial class PasswordReset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_review_books_book_id",
                table: "review");

            migrationBuilder.DropForeignKey(
                name: "fk_review_users_user_id",
                table: "review");

            migrationBuilder.DropPrimaryKey(
                name: "pk_review",
                table: "review");

            migrationBuilder.RenameTable(
                name: "review",
                newName: "reviews");

            migrationBuilder.RenameIndex(
                name: "ix_review_user_id",
                table: "reviews",
                newName: "ix_reviews_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_review_book_id",
                table: "reviews",
                newName: "ix_reviews_book_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_reviews",
                table: "reviews",
                column: "id");

            migrationBuilder.CreateTable(
                name: "password_resets",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    token = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_password_resets", x => x.id);
                    table.ForeignKey(
                        name: "fk_password_resets_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_password_resets_token",
                table: "password_resets",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_password_resets_user_id",
                table: "password_resets",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_reviews_books_book_id",
                table: "reviews",
                column: "book_id",
                principalTable: "books",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_reviews_users_user_id",
                table: "reviews",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_reviews_books_book_id",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "fk_reviews_users_user_id",
                table: "reviews");

            migrationBuilder.DropTable(
                name: "password_resets");

            migrationBuilder.DropPrimaryKey(
                name: "pk_reviews",
                table: "reviews");

            migrationBuilder.RenameTable(
                name: "reviews",
                newName: "review");

            migrationBuilder.RenameIndex(
                name: "ix_reviews_user_id",
                table: "review",
                newName: "ix_review_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_reviews_book_id",
                table: "review",
                newName: "ix_review_book_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_review",
                table: "review",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_review_books_book_id",
                table: "review",
                column: "book_id",
                principalTable: "books",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_review_users_user_id",
                table: "review",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
