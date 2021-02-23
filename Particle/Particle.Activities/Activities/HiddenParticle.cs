using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using Particle.Activities.Properties;
using Particle.Models;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using UiPath.Shared.Activities.Utilities;

namespace Particle.Activities
{
    [LocalizedDisplayName(nameof(Resources.HiddenParticle_DisplayName))]
    [LocalizedDescription(nameof(Resources.HiddenParticle_Description))]
    public class HiddenParticle : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.Timeout_DisplayName))]
        [LocalizedDescription(nameof(Resources.Timeout_Description))]
        public InArgument<int> TimeoutMS { get; set; }

        #endregion


        #region Constructors

        public HiddenParticle()
        {
            Constraints.Add(ActivityConstraints.HasParentType<HiddenParticle, ParticalScope>(string.Format(Resources.ValidationScope_Error, Resources.ParticalScope_DisplayName)));
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Object Container: Use objectContainer.Get<T>() to retrieve objects from the scope
            var objectContainer = context.GetFromContext<IObjectContainer>(ParticalScope.ParentContainerPropertyTag);

            // Inputs
            var timeout = TimeoutMS.Get(context);

            // Set a timeout on the execution
            var task = ExecuteWithTimeout(context, cancellationToken);
            if (await Task.WhenAny(task, Task.Delay(timeout, cancellationToken)) != task) throw new TimeoutException(Resources.Timeout_Error);

            // Outputs
            return (ctx) => {
            };
        }

        private async Task ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var objectContainer = context.GetFromContext<IObjectContainer>(ParticalScope.ParentContainerPropertyTag);

            var view = objectContainer.Get<FireworksWindow>();
            view.TimerEnd();
            view.Visibility = System.Windows.Visibility.Collapsed;
        }

        #endregion
    }
}

