using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfindingTest
{
    public class WaveFunctionCollapseHandler
    {
        WaveFunction[] waveFunctions;
        bool initialized = false;

        public bool waveFunctionCollapsed = false;

        public WaveFunctionCollapseHandler(WaveFunction[] waveFunctions)
        {
            this.waveFunctions = waveFunctions;
            foreach (WaveFunction wf in waveFunctions) wf.collapse += Propagate;
            waveFunctions.Where(x => x.cor == new Point(0, 0)).ElementAt(0).Collapse();

        }

        void Propagate(object function, EventArgs e)
        {
            WaveFunction wf = function as WaveFunction;
            Point left = new Point(wf.cor.X + 1, wf.cor.Y);
            Point down = new Point(wf.cor.X, wf.cor.Y + 1);

            WaveFunction w1 = null;
            if (waveFunctions.Any(x => x.cor == left))
                w1 = waveFunctions.Where(x => x.cor == left).ElementAt(0);

            WaveFunction w2 = null;
            if(waveFunctions.Any(x => x.cor == down))
                w2 = waveFunctions.Where(x => x.cor == down).ElementAt(0);

            if ( w1 != null && ((w1.Path.Count > 0 && w1.pathEntropy < (wf.pathEntropy + w1.entropy)) || w1.Path.Count == 0))
            {
                int t1 = Array.FindIndex(waveFunctions, x => x.Equals(w1));
                w1.pathEntropy = wf.pathEntropy + w1.entropy;
                w1.Path = new List<WaveFunction>(wf.Path);
                w1.Path.Add(w1);
                w1.propagated = true;
                waveFunctions[t1] = w1;
            }

            if ( w2 != null && ((w2.Path.Count > 0 && w2.pathEntropy < (wf.pathEntropy + w2.entropy)) || w2.Path.Count == 0))
            {
                int t2 = Array.FindIndex(waveFunctions, x => x.Equals(w2));
                w2.pathEntropy = wf.pathEntropy + w2.entropy;
                w2.Path = new List<WaveFunction>(wf.Path);
                w2.Path.Add(w2);
                w2.propagated = true;
                waveFunctions[t2] = w2;
            }

            initialized = true;
        }

        public Point[] GetBestPath(Point point)
        {
            WaveFunction wf = waveFunctions.Single(x => x.cor.Equals(point));

            WaveFunction[] w = new WaveFunction[2];
            w[0] = waveFunctions.Single(x => x.cor == new Point(wf.cor.X - 1, wf.cor.Y));
            w[1] = waveFunctions.Single(x => x.cor == new Point(wf.cor.X, wf.cor.Y - 1)); ;

            
            return w.Where(x => x.collapsed).MinBy(x => x.pathEntropy).Path.Select(x => x.cor).ToArray();
        }

        public Point GetNextPoint()
        {
            while(initialized == false)
            {
                Task.Delay(10);
            }

            WaveFunction temp = waveFunctions.Where(x => !x.collapsed && x.propagated).MinBy(x => x.entropy);
            temp.Collapse();

            if (waveFunctions.All(x => x.collapsed))
                waveFunctionCollapsed = true;

            return temp.cor;
        }
    }

    public class WaveFunction
    {
        public int entropy = 0;
        public int pathEntropy = 0;
        public bool collapsed = false;
        public bool propagated = false;
        public event EventHandler collapse;
        public Point cor;
        public List<WaveFunction> Path = new List<WaveFunction>();

        public WaveFunction(int Entropy, Point cor)
        {
            entropy = Entropy;
            this.cor = cor;
        }

        public void Collapse()
        {
            collapsed = true;
            collapse.Invoke(this, EventArgs.Empty);
        }
    }
}
