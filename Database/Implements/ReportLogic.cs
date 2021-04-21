using BusinessLogica.Enum;
using BusinessLogica.HelperModels;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database.Implements
{
    public class ReportLogic
    {


        public static void CreateDoc(PdfInfo info)
        {
            Document document = new Document();
            DefineStyles(document);
            Section section = document.AddSection();
            Paragraph paragraph = section.AddParagraph(info.Title);
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.Style = "NormalTitle";
            if (info.id==1)
            {
                Paragraph paragraph1 = section.AddParagraph("Договор заключен на имя "+info.Dogovors[0].FIO+ " На сумму "+info.Dogovors[0].Cena);
                paragraph1.Format.Alignment = ParagraphAlignment.Center;
                paragraph1.Style = "NormalTitle";
                var doctorTable = document.LastSection.AddTable();
                var headerWidths = new List<string> { "4cm", "4cm", "4cm", "4cm" };
                foreach (var elem in headerWidths)
                {
                    doctorTable.AddColumn(elem);
                }
                CreateRow(new PdfRowParameters
                {
                    Table = doctorTable,
                    Texts = new List<string> { "Product", "Count", "Цена за штуку","Сумма" },
                    Style = "NormalTitle",
                    ParagraphAlignment = ParagraphAlignment.Center
                });
                foreach (var dogproduct in info.DogovorProduct)
                {
                    foreach (var product in info.Products)
                    {
                        if (dogproduct.ProductId==product.Id) {
                            CreateRow(new PdfRowParameters
                            {
                                Table = doctorTable,
                                Texts = new List<string> { product.Name, Convert.ToString(dogproduct.Count), Convert.ToString(product.Cena), Convert.ToString(dogproduct.Count * product.Cena) },
                                Style = "NormalTitle",
                                ParagraphAlignment = ParagraphAlignment.Center
                            }); }
                    }
                 
                }
                Paragraph paragraph3 = section.AddParagraph("Data: " + info.Dogovors[0].Data.ToShortDateString());
                paragraph3.Format.Alignment = ParagraphAlignment.Right;
                paragraph3.Style = "NormalTitle";
                Paragraph paragraph2 = section.AddParagraph("Ответсвенный: " + info.User[0].Login);
                paragraph2.Format.Alignment = ParagraphAlignment.Right;
                paragraph2.Style = "NormalTitle";
            }
            if (info.id==0)
            {
                var doctorTable = document.LastSection.AddTable();
                var headerWidths = new List<string> { "3cm", "3cm", "4cm", "4cm", "3cm" };
                foreach (var elem in headerWidths)
                {
                    doctorTable.AddColumn(elem);
                }
                CreateRow(new PdfRowParameters
                {
                    Table = doctorTable,
                    Texts = new List<string> { "Номер", "Дата", "Клиент", "Ответственный", "Сумма" },
                    Style = "NormalTitle",
                    ParagraphAlignment = ParagraphAlignment.Center
                });
                foreach (var dogovor in info.Dogovors)
                {
                    foreach (var user in info.User)
                    {
                        if (dogovor.UserId == user.Id)
                        {
                            CreateRow(new PdfRowParameters
                            {
                                Table = doctorTable,
                                Texts = new List<string> { Convert.ToString(dogovor.Id),Convert.ToString(dogovor.Data.ToShortDateString()),dogovor.FIO, user.Login, Convert.ToString(dogovor.Cena) },
                                Style = "NormalTitle",
                                ParagraphAlignment = ParagraphAlignment.Center
                            });
                        }
                    }

                }
                CreateRow(new PdfRowParameters
                {
                    Table = doctorTable,
                    Texts = new List<string> { "Итого:", "","","", Convert.ToString(info.Dogovors.Sum(rec=>rec.Cena))},
                    Style = "NormalTitle",
                    ParagraphAlignment = ParagraphAlignment.Center
                });

            }
            if (info.id == -1)
            {
                var doctorTable = document.LastSection.AddTable();
                var headerWidths = new List<string> { "3cm", "3cm", "4cm", "4cm", "3cm" };
                foreach (var elem in headerWidths)
                {
                    doctorTable.AddColumn(elem);
                }
                CreateRow(new PdfRowParameters
                {
                    Table = doctorTable,
                    Texts = new List<string> { "Склад", "Продукт", "Перемещение", "Цена", "Сумма" },
                    Style = "NormalTitle",
                    ParagraphAlignment = ParagraphAlignment.Center
                });
                foreach (var sklad in info.Sklad)
                {
                    if(info.SkladProduct.Count(rec => rec.SkladId == sklad.Id) != 0)
                    {
                        CreateRow(new PdfRowParameters
                        {
                            Table = doctorTable,
                            Texts = new List<string> { sklad.Name,"","","","" },
                            Style = "NormalTitle",
                            ParagraphAlignment = ParagraphAlignment.Center
                        });
                    }

                    var skladProduct = info.SkladProduct.Where(rec=>rec.SkladId==sklad.Id);
                    
                    foreach (var sproduct in skladProduct)
                    {
                        foreach (var product in info.Products)
                        {
                            if (sproduct.ProductId == product.Id)
                            {
                                var k = 1;
                                if (sproduct.Status==Status.Списание)
                                {
                                    k = -1;
                                }
                                CreateRow(new PdfRowParameters
                                {
                                    Table = doctorTable,
                                    Texts = new List<string> { "", product.Name, Convert.ToString(k*sproduct.Count),Convert.ToString( product.Cena), Convert.ToString(sproduct.Count*product.Cena) },
                                    Style = "NormalTitle",
                                    ParagraphAlignment = ParagraphAlignment.Center
                                });
                            }
                        }
                    }

                }

            }
            if (info.id == -2)
            {
                var doctorTable = document.LastSection.AddTable();
                var headerWidths = new List<string> { "3cm", "3cm", "4cm", "4cm" };
                foreach (var elem in headerWidths)
                {
                    doctorTable.AddColumn(elem);
                }
                CreateRow(new PdfRowParameters
                {
                    Table = doctorTable,
                    Texts = new List<string> { "Сотрудник", "Договор", "Дата", "Сумма" },
                    Style = "NormalTitle",
                    ParagraphAlignment = ParagraphAlignment.Center
                });
                foreach (var user in info.User)
                {
                    if (info.Dogovors.Count(rec => rec.UserId == user.Id) != 0)
                    {
                        CreateRow(new PdfRowParameters
                        {
                            Table = doctorTable,
                            Texts = new List<string> { user.Login,  "", "", "" },
                            Style = "NormalTitle",
                            ParagraphAlignment = ParagraphAlignment.Center
                        });
                    }
                    var dogovors = info.Dogovors.Where(rec => rec.UserId == user.Id);
                    foreach (var dogovor in dogovors)
                    {
                                CreateRow(new PdfRowParameters
                                {
                                    Table = doctorTable,
                                    Texts = new List<string> { "", Convert.ToString(dogovor.Id), Convert.ToString(dogovor.Data.ToShortDateString()), Convert.ToString(dogovor.Cena) },
                                    Style = "NormalTitle",
                                    ParagraphAlignment = ParagraphAlignment.Center
                                });                    
                    }
                    CreateRow(new PdfRowParameters
                    {
                        Table = doctorTable,
                        Texts = new List<string> { "Итого", "", "", Convert.ToString(dogovors.Sum(rec => rec.Cena)) },
                        Style = "NormalTitle",
                        ParagraphAlignment = ParagraphAlignment.Center
                    });

                }

            }
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true)
            {
                Document = document
            };
            renderer.RenderDocument();
            renderer.PdfDocument.Save(info.FileName);
        }
        private static void DefineStyles(Document document)
        {
            Style style = document.Styles["Normal"];
            style.Font.Name = "Times New Roman";
            style.Font.Size = 14;
            style = document.Styles.AddStyle("NormalTitle", "Normal");
            style.Font.Bold = true;
        }
        private static void CreateRow(PdfRowParameters rowParameters)
        {
            Row row = rowParameters.Table.AddRow();
            for (int i = 0; i < rowParameters.Texts.Count; ++i)
            {
                FillCell(new PdfCellParameters
                {
                    Cell = row.Cells[i],
                    Text = rowParameters.Texts[i],
                    Style = rowParameters.Style,
                    BorderWidth = 0.5,
                    ParagraphAlignment = rowParameters.ParagraphAlignment
                });
            }
        }
        private static void FillCell(PdfCellParameters cellParameters)
        {
            cellParameters.Cell.AddParagraph(cellParameters.Text);
            if (!string.IsNullOrEmpty(cellParameters.Style))
            {
                cellParameters.Cell.Style = cellParameters.Style;
            }
            cellParameters.Cell.Borders.Left.Width = cellParameters.BorderWidth;
            cellParameters.Cell.Borders.Right.Width = cellParameters.BorderWidth;
            cellParameters.Cell.Borders.Top.Width = cellParameters.BorderWidth;
            cellParameters.Cell.Borders.Bottom.Width = cellParameters.BorderWidth;
            cellParameters.Cell.Format.Alignment = cellParameters.ParagraphAlignment;
            cellParameters.Cell.VerticalAlignment = VerticalAlignment.Center;
        }
    }
}
