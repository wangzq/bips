using System.Collections.Generic;
using System.Linq;
using CodeOwls.PowerShell.Provider.PathNodeProcessors;
using CodeOwls.PowerShell.Provider.PathNodes;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;

namespace CodeOwls.BIPS
{
    public class DataFlowComponentNodeFactory : NodeFactoryBase
    {
        private readonly IDTSComponentMetaData100 _input;

        public DataFlowComponentNodeFactory(IDTSComponentMetaData100 input)
        {
            _input = input;
        }

        public override IEnumerable<INodeFactory> GetNodeChildren(IContext context)
        {
            var nodes = new List<INodeFactory>();

            nodes.Add(new CollectionNodeFactory<IDTSInput100>("Inputs", _input.InputCollection.Cast<IDTSInput100>(), c => new DataFlowInputNodeFactory(c)));
            nodes.Add(new CollectionNodeFactory<IDTSOutput100>("Outputs", _input.OutputCollection.Cast<IDTSOutput100>(), c => new DataFlowOutputNodeFactory(c)));
            nodes.Add(new CollectionNodeFactory<IDTSRuntimeConnection100>("Connections", _input.RuntimeConnectionCollection.Cast<IDTSRuntimeConnection100>(), c => new DataFlowConnectionNodeFactory(c)));
            nodes.Add(new CollectionNodeFactory<IDTSCustomProperty100>("Properties", _input.CustomPropertyCollection.Cast<IDTSCustomProperty100>(), c => new DataFlowPropertyNodeFactory(c)));

            return nodes;
        }

        public override IPathNode GetNodeValue()
        {
            return new ContainerPathNode( _input, Name );
        }

        public override string Name
        {
            get { return _input.Name; }
        }
    }
}