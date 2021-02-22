using System;
using System.Activities;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Particle.Activities.Properties;
using Particle.Enums;
using Particle.Models;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using UiPath.Shared.Activities.Utilities;

namespace Particle.Activities
{
    [LocalizedDisplayName(nameof(Resources.ShowParticle_DisplayName))]
    [LocalizedDescription(nameof(Resources.ShowParticle_Description))]
    public class ShowParticle : ContinuableAsyncCodeActivity
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
        public InArgument<int> TimeoutMS { get; set; } = 60000;

        [LocalizedDisplayName(nameof(Resources.ParticalScope_ParticalType_DisplayName))]
        [LocalizedDescription(nameof(Resources.ParticalScope_ParticalType_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        [TypeConverter(typeof(EnumNameConverter<ParticleType>))]
        public ParticleType ParticalType { get; set; }

        [LocalizedDisplayName(nameof(Resources.ParticalScope_AutoHiddenTime_DisplayName))]
        [LocalizedDescription(nameof(Resources.ParticalScope_AutoHiddenTime_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<TimeSpan> AutoHiddenTime { get; set; }

        #endregion


        #region Constructors

        public ShowParticle()
        {
            Constraints.Add(ActivityConstraints.HasParentType<ShowParticle, ParticalScope>(string.Format(Resources.ValidationScope_Error, Resources.ParticalScope_DisplayName)));
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (ParticalType == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(ParticalType)));

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
            var autohiddentime = AutoHiddenTime.Get(context);
            var particaltype = this.ParticalType;

            var view = objectContainer.Get<FireworksWindow>();
            view.TimerStart(particaltype,autohiddentime);
            view.Visibility = System.Windows.Visibility.Visible;
            view.Show();
        }

        #endregion
    }
}

