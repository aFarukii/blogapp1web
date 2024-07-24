/** @type {import('tailwindcss').Config} */
const plugin = require('tailwindcss/plugin')
module.exports = {
    content: [
        "./Views/*.cshtml",
        "./Views/Auth/*.cshtml",
        "./Views/Home/*.cshtml",
        "./Views/Shared/*.cshtml",],
    theme: {
        container: {
            center: true,
            screens: {
                lg: '1140px',
                xl: '1140px',
                '2xl': '1140px',
            }
        }, 
    extend: {
            fontFamily: {
                Abel: ["Abel", "sans-serif"],
                oswald: ["Oswald", 'sans-serif']
            },
            colors: {
                'my-black': '#1A2130',
                'navy-blue': '#5A72A0',
                'my-blue': '#83B4FF',
                'my-yellow': '#FDFFE2'
            },
            textShadow: {
                sm: '0 1px 2px var(--tw-shadow-color)',
                DEFAULT: '0 2px 4px var(--tw-shadow-color)',
                lg: '0 8px 16px var(--tw-shadow-color)',
            },
        }
    },
    plugins: [
        require('@tailwindcss/forms'),
        require('@tailwindcss/typography'),
        plugin(function ({ matchUtilities, theme }) {
            matchUtilities(
                {
                    'text-shadow': (value) => ({
                        textShadow: value,
                    }),
                },
                { values: theme('textShadow') }
            )
        }),
    ],
}

