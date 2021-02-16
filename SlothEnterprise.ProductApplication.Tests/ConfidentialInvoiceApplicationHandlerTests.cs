using FluentAssertions;
using Moq;
using SlothEnterprise.External;
using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.ApplicationHandlers;
using SlothEnterprise.ProductApplication.Applications;
using SlothEnterprise.ProductApplication.Products;
using Xunit;

namespace SlothEnterprise.ProductApplication.Tests
{
    public class ConfidentialInvoiceApplicationHandlerTests
    {
        private readonly Mock<IConfidentialInvoiceService> _confidentialInvoiceServiceMock;
        private readonly Mock<ISellerApplication> _sellerApplicationMock;
        private readonly Mock<IApplicationResult> _applicationResultMock;

        private readonly ConfidentialInvoiceApplicationHandler _sut;

        public ConfidentialInvoiceApplicationHandlerTests()
        {
            _confidentialInvoiceServiceMock = new Mock<IConfidentialInvoiceService>();

            _sellerApplicationMock = new Mock<ISellerApplication>();
            _sellerApplicationMock.SetupProperty(p => p.Product, new ConfidentialInvoiceDiscount());
            _sellerApplicationMock.SetupProperty(p => p.CompanyData, new SellerCompanyData());

            _applicationResultMock = new Mock<IApplicationResult>();
            _applicationResultMock.SetupProperty(x => x.ApplicationId, 1);
            _applicationResultMock.SetupProperty(x => x.Success, true);

            _sut = new ConfidentialInvoiceApplicationHandler(_confidentialInvoiceServiceMock.Object);
        }

        [Fact]
        public void ConfidentialInvoiceApplicationHandler_WhenCalledWithConfidentialInvoiceDiscount_ShouldReturnOne()
        {
            _confidentialInvoiceServiceMock.Setup(m => m.SubmitApplicationFor(It.IsAny<CompanyDataRequest>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>()))
                .Returns(_applicationResultMock.Object);
            var result = _sut.Handle(_sellerApplicationMock.Object);
            result.Should().Be(1);
        }
    }
}
