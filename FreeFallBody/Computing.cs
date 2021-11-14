using System.Collections.Generic;
using System;

namespace WindowsFormsApp1
{
    class Computing
    {
        const double g = 9.8f;
        const double HemisphereCoefficient = 0.4f, BallCoefficient = 1.22f, DripBodyCoefficient = 0.045f;
        private double _mass, _height, _circumference, _tau, _coefficient, _radius, _resistance, _time = 0, _mDensity, _eDensity, _viscous, _vMass1, _vMass2;
        private List<double> v, h, t;
        int i = 0;

        private Computing() {}

        /// <summary>
        /// Using for computing speed, heght and time for non-parachute
        /// </summary>
        /// <param name="mass">Mass of parachutist</param>
        /// <param name="height">Heght of parachutist</param>
        /// <param name="circumference">Circumference of parachutist</param>
        /// <param name="resistance">Environmental resistance</param>
        public Computing(double mass, double height, double circumference, double resistance)
        {
            _mass = mass;
            _height = height;
            _circumference = circumference;
            _resistance = resistance;
        }

        /// <summary>
        /// Using for computing speed, height and time for parachute
        /// </summary>
        /// <param name="radius">Radius of parachutist</param>
        /// <param name="mass">Mass of parachutist</param>
        /// <param name="resistance">Environmental resistance</param>
        public Computing(double radius, double mass, double resistance)
        {
            _radius = radius;
            _mass = mass;
            _resistance = resistance;
        }

        /// <summary>
        /// Using for computing speed, height and time for a ball in a viscous environment
        /// </summary>
        /// <param name="m_density">Metal density</param>
        /// <param name="e_density">Environment density</param>
        /// <param name="viscous">Dynamic viscous of environment</param>
        /// <param name="radius">Radius of the ball</param>
        /// <param name="mass1">Weight computing with this formula: 4/3 * pi * R^3 * (m_density - e_density)</param>
        /// <param name="mass2">Weight computing with this formula: 4/3 * pi * R^3 * m_density</param>
        public Computing(double m_density, double e_density, double viscous, double radius, double mass1, double mass2)
        {
            _mDensity = m_density;
            _eDensity = e_density;
            _viscous = viscous;
            _radius = radius;
            _vMass1 = mass1;
            _vMass2 = mass2;

        }

        public void WithoutParachute()
        {
            v = new List<double>();
            h = new List<double>();
            t = new List<double>();

            v.Add(0);
            h.Add(0);
            t.Add(0);

            _coefficient = 0.5f * BallCoefficient * (_height * _circumference) * _resistance;

            _tau = _coefficient * 0.1f;

            double F = _mass * g;

            i = 0;

            while (true)
            {
                v.Add(v[i] + (_tau / 2) * ((F - _coefficient * Math.Pow(v[i], 2)) / _mass + (F - _coefficient * Math.Pow((v[i] + _tau * (F - _coefficient * Math.Pow(v[i], 2)) / _mass), 2)) / _mass));

                h.Add(h[i] + v[i + 1] * _tau);

                t.Add(_time + _tau);

                if (v[i + 1] - v[i] <= 0.01f) break;
                
                i++;
            }
        }

        public void WithParachute()
        {
            v = new List<double>();
            h = new List<double>();
            t = new List<double>();

            v.Add(0);
            h.Add(0);
            t.Add(0);

            _coefficient = 0.5f * HemisphereCoefficient * _radius * _resistance;

            _tau = _coefficient * 0.1f;

            double F = _mass * g;

            i = 0;

            while (true)
            {
                v.Add(v[i] + (_tau / 2) * ((F - _coefficient * Math.Pow(v[i], 2)) / _mass + (F - _coefficient * Math.Pow((v[i] + _tau * (F - _coefficient * Math.Pow(v[i], 2)) / _mass), 2)) / _mass));

                h.Add(h[i] + v[i + 1] * _tau);

                t.Add(_time + _tau / 0.1f);

                if (v[i + 1] - v[i] <= 0.01f) break;

                i++;
            }
        }

        public void ViscousMedium()
        {
            v = new List<double>();
            h = new List<double>();
            t = new List<double>();

            v.Add(0);
            h.Add(0);
            t.Add(0);

            _coefficient = 6 * Math.PI * _viscous * _radius;

            _tau = _coefficient * 0.0001f - 0.1f;

            double F = _vMass1 * g;

            i = 0;

            while (true)
            {
                v.Add(v[i] + (_tau / 2) * ((F - _coefficient * v[i]) / _vMass2 + (F - _coefficient * (v[i] + _tau * (F - _coefficient * v[i]) / _vMass2)) / _vMass2));

                h.Add(h[i] + v[i + 1] * _tau);

                t.Add(_time + _tau);

                if (v[i + 1] - v[i] <= 0.01f) break;

                i++;
            }
        }

        public List<double> GetSpeed()
        {
            return v;
        }

        public List<double> GetHeight()
        {
            return h;
        }

        public List<double> GetTime()
        {
            return t;
        }
    }
}
