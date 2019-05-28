using System.ComponentModel;
using GalaSoft.MvvmLight;
using Telerik.JustMock;
using Xunit;

namespace Trello.ViewModel
{
    public class MainViewModelTests
    {
        /// <summary>
        /// The view relies on this event to know when to update the screen.
        /// </summary>
        [Fact]
        public void Event_is_raised_if_the_CurrentViewModel_is_changed()
        {
            // Arrange
            var propertyChangedEventRaised = false;
            var subjectUnderTest = new MainViewModel();
            subjectUnderTest.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                Assert.Equal(subjectUnderTest, sender);
                Assert.Equal("CurrentViewModel", e.PropertyName);
                propertyChangedEventRaised = true;
            };

            // Act
            subjectUnderTest.CurrentViewModel = Mock.Create<ViewModelBase>();

            // Assert
            Assert.True(propertyChangedEventRaised);
        }

        [Fact]
        public void Unnecessary_event_is_not_raised_if_the_CurrentViewModel_is_set_to_the_same_instance()
        {
            // Arrange
            var propertyChangedEventRaised = false;
            var sameInstance = Mock.Create<ViewModelBase>();
            var subjectUnderTest = new MainViewModel { CurrentViewModel = sameInstance };
            subjectUnderTest.PropertyChanged += delegate { propertyChangedEventRaised = true; };

            // Act
            subjectUnderTest.CurrentViewModel = sameInstance;

            // Assert
            Assert.False(propertyChangedEventRaised);
        }
    }
}
