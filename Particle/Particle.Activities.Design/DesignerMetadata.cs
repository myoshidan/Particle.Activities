using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using Particle.Activities.Design.Designers;
using Particle.Activities.Design.Properties;

namespace Particle.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(ParticalScope), categoryAttribute);
            builder.AddCustomAttributes(typeof(ParticalScope), new DesignerAttribute(typeof(ParticalScopeDesigner)));
            builder.AddCustomAttributes(typeof(ParticalScope), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(ShowParticle), categoryAttribute);
            builder.AddCustomAttributes(typeof(ShowParticle), new DesignerAttribute(typeof(ShowParticleDesigner)));
            builder.AddCustomAttributes(typeof(ShowParticle), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(HiddenParticle), categoryAttribute);
            builder.AddCustomAttributes(typeof(HiddenParticle), new DesignerAttribute(typeof(HiddenParticleDesigner)));
            builder.AddCustomAttributes(typeof(HiddenParticle), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
