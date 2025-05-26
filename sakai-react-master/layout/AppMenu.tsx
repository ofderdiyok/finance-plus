/* eslint-disable @next/next/no-img-element */

import React, { useContext } from 'react';
import AppMenuitem from './AppMenuitem';
import { LayoutContext } from './context/layoutcontext';
import { MenuProvider } from './context/menucontext';
import Link from 'next/link';
import { AppMenuItem } from '@/types';

const AppMenu = () => {
    const { layoutConfig } = useContext(LayoutContext);

    const model: AppMenuItem[] = [
        {
            label: 'Home',
            items: [{ label: 'Ana Sayfa', icon: 'pi pi-fw pi-home', to: '/' }]
        },
        {
            label: 'Finansal İşlemlerim',
            items: [
                {
                    label: 'Transferlerim',
                    icon: 'pi pi-fw pi-money-bill',
                    to: '/transferlerim'
                },
                {
                    label: 'Harcamalarım',
                    icon: 'pi pi-fw pi-wallet',
                    to: '/harcamalarim'
                }
            ]
        },
        
    ];

    return (
        <MenuProvider>
            <ul className="layout-menu">
                {model.map((item, i) => {
                    return !item?.seperator ? <AppMenuitem item={item} root={true} index={i} key={item.label} /> : <li className="menu-separator"></li>;
                })}
            </ul>
        </MenuProvider>
    );
};

export default AppMenu;
