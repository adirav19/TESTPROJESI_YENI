using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using TESTPROJESI.Business.DTOs;
using TESTPROJESI.Controllers;
using TESTPROJESI.Models;
using TESTPROJESI.Services.Interfaces;
using Xunit;

namespace TESTPROJESI.Tests
{
    public class ControllerIndexTests
    {
        [Fact]
        public void FinishedGoods_Index_ReturnsView()
        {
            var controller = new FinishedGoodsController(
                new FakeFinishedGoodsService(),
                NullLogger<FinishedGoodsController>.Instance);

            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task FinishedGoods_GetAll_ReturnsSuccessResponse()
        {
            var service = new FakeFinishedGoodsService();
            var controller = new FinishedGoodsController(
                service,
                NullLogger<FinishedGoodsController>.Instance);

            var result = await controller.GetAll();

            var json = Assert.IsType<JsonResult>(result);
            var response = Assert.IsType<ApiResponse<List<FinishedGoodsCreateDto>>>(json.Value);

            Assert.True(response.Success);
            Assert.Equal(service.Items.Count, response.Data?.Count);
        }

        [Fact]
        public async Task ProductionFlow_Index_ReturnsViewWithModel()
        {
            var service = new FakeProductionFlowService();
            var controller = new ProductionFlowController(
                service,
                NullLogger<ProductionFlowController>.Instance);

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Same(service.Items, viewResult.Model);
        }
    }

    internal sealed class FakeFinishedGoodsService : IFinishedGoodsService
    {
        public List<FinishedGoodsCreateDto> Items { get; } =
        [
            new FinishedGoodsCreateDto
            {
                FisNo = "FG-001",
                Tarih = "2024-01-01",
                Depo = "1",
                Malzeme = "MAL-001",
                Miktar = 10,
                Birim = "Adet"
            }
        ];

        public Task<ApiResponse<JsonElement>> CreateAsync(FinishedGoodsCreateDto dto) =>
            throw new NotImplementedException();

        public Task<ApiResponse<bool>> DeleteAsync(string fisNo) =>
            throw new NotImplementedException();

        public Task<List<FinishedGoodsCreateDto>> GetAllAsync(string? queryParams = null) =>
            Task.FromResult(Items);

        public Task<FinishedGoodsDetailDto?> GetByIdAsync(string fisNo) =>
            throw new NotImplementedException();

        public Task<ApiResponse<JsonElement>> UpdateAsync(FinishedGoodsUpdateDto dto) =>
            throw new NotImplementedException();

        public Task<ApiResponse<JsonElement>> UpdateQuantityAsync(KalemDto dto) =>
            throw new NotImplementedException();
    }

    internal sealed class FakeProductionFlowService : IProductionFlowService
    {
        public List<ProductionFlowDto> Items { get; } =
        [
            new ProductionFlowDto
            {
                IsEmriNo = "IS-001",
                CONFSIRANO = "1",
                IncKeyNo = 1,
                StokKodu = "ST-001",
                OpKodu = "OP-001",
                OPSIRANO = "1",
                IstasyonKodu = "IST-001",
                SIMULTANEOPR = 0,
                MRPMAKINENO = 1,
                MRPISCINO = 1,
                BASLANGICTARIH = "2024-01-01",
                BASLANGICVARDIYA = 1,
                SURE = 1,
                SURETIPI = 1,
                BITISTARIHSAAT = "2024-01-01",
                AKTIVITEKODU = "AK-001",
                ARIZAKODU = "",
                ISLENDI = false,
                URETILENMIKTAR = 1,
                FIREMIKTAR = 0,
                ProjeKodu = "PR-001",
                RevNo = "1",
                SERILOTNO = "SL-001",
                USKDEPOKODU = 1,
                ACIK1 = string.Empty,
                ACIK2 = string.Empty,
                ACIK3 = string.Empty,
                ACIK4 = string.Empty,
                STOKURS_INCKEYNO = string.Empty,
                YapKod = string.Empty,
                OPR_KAYNAK_VIEW = string.Empty,
                BASLADI_BITMEDI = false,
                SUBISEMRI_NO = string.Empty,
                UAKKaynakListCount = 0,
                OLCUBRMIKTAR = 0,
                OLCUBRFIRE = 0
            }
        ];

        public Task<JsonElement> CreateAsync(ProductionFlowDto dto) =>
            throw new NotImplementedException();

        public Task DeleteAsync(int id) => throw new NotImplementedException();

        public Task<JsonElement> CreateFinishedGoodsReceiptAsync(FinishedGoodsReceiptParamDto param) =>
            throw new NotImplementedException();

        public Task<List<ProductionFlowDto>> GetAllAsync(string? queryParams = null) =>
            Task.FromResult(Items);

        public Task<ProductionFlowDto> GetByIdAsync(int id) =>
            throw new NotImplementedException();
    }
}
