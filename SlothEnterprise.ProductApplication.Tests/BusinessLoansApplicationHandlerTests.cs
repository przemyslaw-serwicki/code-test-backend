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
    public class BusinessLoansApplicationHandlerTests
    {
        private readonly Mock<IBusinessLoansService> _businessLoansServiceMock;
        private readonly Mock<ISellerApplication> _sellerApplicationMock;
        private readonly Mock<IApplicationResult> _applicationResultMock;

        private readonly BusinessLoansApplicationHandler _sut;

        public BusinessLoansApplicationHandlerTests()
        {
            _businessLoansServiceMock = new Mock<IBusinessLoansService>();

            _sellerApplicationMock = new Mock<ISellerApplication>();
            _sellerApplicationMock.SetupProperty(p => p.Product, new BusinessLoans());
            _sellerApplicationMock.SetupProperty(p => p.CompanyData, new SellerCompanyData());

            _applicationResultMock = new Mock<IApplicationResult>();
            _applicationResultMock.SetupProperty(x => x.ApplicationId, 1);
            _applicationResultMock.SetupProperty(x => x.Success, true);

            _sut = new BusinessLoansApplicationHandler(_businessLoansServiceMock.Object);
        }

        [Fact]
        public void BusinessLoansApplicationHandler_WhenCalledWithBusinessLoans_ShouldReturnOne()
        {
            _businessLoansServiceMock.Setup(m => m.SubmitApplicationFor(It.IsAny<CompanyDataRequest>(), It.IsAny<LoansRequest>()))
                .Returns(_applicationResultMock.Object);
            var result = _sut.Handle(_sellerApplicationMock.Object);
            result.Should().Be(1);
        }

        //TODO: More tests that check other cases like
        //- value of result when ApplicationResult has success but ApplicationId is NULL
        //- thrown exception 'Can not create CompanyDataRequest' when CompanyData is null
    }
}
