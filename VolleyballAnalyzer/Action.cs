using System.ComponentModel;

namespace VolleyballAnalyzer
{
        public enum Action
        {
            [Description("Napad - uspeh")] NapadSuccess,
            [Description("Napad - napaka")] NapadFail,
            [Description("Sprejem - uspeh")] SprejemSuccess,
            [Description("Sprejem - napaka")] SprejemFail,
            [Description("Servis - uspeh")] ServisSuccess,
            [Description("Servis - napaka")] ServisFail,
            [Description("Podaja - uspeh")] PodajaSuccess,
            [Description("Podaja - napaka")] PodajaFail,
            [Description("Blok - uspeh")] BlokSuccess,
            [Description("Blok - napaka")] BlokFail,
            Prestop,
            Mreza,
            [Description("Druga napaka")] Drugo,
            Menjava
        }
}
