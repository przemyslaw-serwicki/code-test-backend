using FluentAssertions;
using Moq;
using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.ApplicationHandlers;
using SlothEnterprise.ProductApplication.Applications;
using SlothEnterprise.ProductApplication.Products;
using Xunit;

namespace SlothEnterprise.ProductApplication.Tests
{
    public class SelectiveInvoiceApplicationHandlerTests
    {
        private readonly Mock<ISelectInvoiceService> _selectInvoiceServiceMock;
        private readonly Mock<ISellerApplication> _sellerApplicationMock;

        private readonly SelectiveInvoiceApplicationHandler _sut;

        public SelectiveInvoiceApplicationHandlerTests()
        {
            _selectInvoiceServiceMock = new Mock<ISelectInvoiceService>();

            _sellerApplicationMock = new Mock<ISellerApplication>();
            _sellerApplicationMock.SetupProperty(p => p.Product, new SelectiveInvoiceDiscount());
            _sellerApplicationMock.SetupProperty(p => p.CompanyData, new SellerCompanyData());

            _sut = new SelectiveInvoiceApplicationHandler(_selectInvoiceServiceMock.Object);
        }

        [Fact]
        public void SelectiveInvoiceApplicationHandler_WhenCalledWithSelectiveInvoiceDiscount_ShouldReturnOne()
        {
            _selectInvoiceServiceMock.Setup(m => m.SubmitApplicationFor(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>()))
                .Returns(1);
            var result = _sut.Handle(_sellerApplicationMock.Object);
            result.Should().Be(1);
        }
    }
}
