using System;
using System.Collections.Generic;
using App.Metrics.MetricData;

namespace App.Metrics.Internal
{
    internal sealed class DefaultMetricsFilter : IMetricsFilter
    {
        public static IMetricsFilter All = new NoOpFilter();
        private Predicate<string> _context;
        private Predicate<string> _name;
        private HashSet<MetricType> _types;

        public bool IsMatch(string context)
        {
            return _context == null || _context(context);
        }

        public bool IsMatch(GaugeValueSource gauge)
        {
            if (_types != null && !_types.Contains(MetricType.Gauge))
            {
                return false;
            }
            return IsNameMatch(gauge.Name);
        }

        public bool IsMatch(CounterValueSource counter)
        {
            if (_types != null && !_types.Contains(MetricType.Counter))
            {
                return false;
            }
            return IsNameMatch(counter.Name);
        }

        public bool IsMatch(MeterValueSource meter)
        {
            if (_types != null && !_types.Contains(MetricType.Meter))
            {
                return false;
            }
            return IsNameMatch(meter.Name);
        }

        public bool IsMatch(HistogramValueSource histogram)
        {
            if (_types != null && !_types.Contains(MetricType.Histogram))
            {
                return false;
            }
            return IsNameMatch(histogram.Name);
        }

        public bool IsMatch(TimerValueSource timer)
        {
            if (_types != null && !_types.Contains(MetricType.Timer))
            {
                return false;
            }
            return IsNameMatch(timer.Name);
        }

        public DefaultMetricsFilter WhereContext(Predicate<string> condition)
        {
            _context = condition;
            return this;
        }

        public DefaultMetricsFilter WhereContext(string context)
        {
            return WhereContext(c => c.Equals(context, StringComparison.OrdinalIgnoreCase));
        }

        public DefaultMetricsFilter WhereName(Predicate<string> condition)
        {
            _name = condition;
            return this;
        }

        public DefaultMetricsFilter WhereNameStartsWith(string name)
        {
            return WhereName(n => n.StartsWith(name, StringComparison.OrdinalIgnoreCase));
        }

        public DefaultMetricsFilter WhereType(params MetricType[] types)
        {
            _types = new HashSet<MetricType>(types);
            return this;
        }

        private bool IsNameMatch(string name)
        {
            return _name == null || _name(name);
        }

        private class NoOpFilter : IMetricsFilter
        {
            public bool IsMatch(string context)
            {
                return true;
            }

            public bool IsMatch(GaugeValueSource gauge)
            {
                return true;
            }

            public bool IsMatch(CounterValueSource counter)
            {
                return true;
            }

            public bool IsMatch(MeterValueSource meter)
            {
                return true;
            }

            public bool IsMatch(HistogramValueSource histogram)
            {
                return true;
            }

            public bool IsMatch(TimerValueSource timer)
            {
                return true;
            }
        }
    }
}